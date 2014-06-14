using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using CustomerTracker.Web.App_Start;
using DotNetOpenAuth.AspNet;
using CustomerTracker.Web.Business.UserBusiness;
using CustomerTracker.Web.Utilities;
using Microsoft.Web.WebPages.OAuth;
using Ninject;

namespace CustomerTracker.Web.Infrastructure.Membership
{
    public class CustomOAuthProvider : IOpenAuthDataProvider, ICustomOAuthProvider
    {
        private IUserUtility _userUtility;

        private readonly IApplicationEnvironment _applicationEnvironment;

        private static Dictionary<string, AuthenticationClientData> _authenticationClients = new Dictionary<string, AuthenticationClientData>(StringComparer.OrdinalIgnoreCase);

        public CustomOAuthProvider()
        {
            _applicationEnvironment = new AspnetEnvironment();

            _userUtility = NinjectWebCommon.GetKernel.Get<IUserUtility>();

        }

        public ICollection<AuthenticationClientData> RegisteredClientData
        {
            get { return _authenticationClients.Values; }
        }

        public AuthenticationResult VerifyOAuthAuthentication(string returnUrl)
        {
            string providerName = _applicationEnvironment.GetOAuthPoviderName();

            if (String.IsNullOrEmpty(providerName))
                return AuthenticationResult.Failed;

            var client = _authenticationClients[providerName];

            return _applicationEnvironment.VerifyAuthentication(client.AuthenticationClient, this, returnUrl);
        }

        public bool OAuthLogin(string provider, string providerUserId, bool persistCookie)
        { 
            var user = _userUtility.FindUserBySocialAccount(provider, providerUserId);

            if (user==null) return false;
            
            WebSecurity.CreateCookieWithUser(user);

            return true; 
              
        }

        public AuthenticationClientData GetOAuthClientData(string providerName)
        {
            return _authenticationClients[providerName];
        }

        public string GetUserNameFromOpenAuth(string provider, string providerUserId)
        {
            var user = _userUtility.FindUserBySocialAccount(provider, providerUserId);

            return user != null ? user.Username : String.Empty;
        }

        public void RequestOAuthAuthentication(string provider, string returnUrl)
        {
            AuthenticationClientData client = _authenticationClients[provider];

            _applicationEnvironment.RequestAuthentication(client.AuthenticationClient, this, returnUrl);

        }

        public static void RegisterClient(IAuthenticationClient client, string displayName, IDictionary<string, object> extraData)
        {
            var clientData = new AuthenticationClientData(client, displayName, extraData);

            if (_authenticationClients.Any(q => q.Key == client.ProviderName) == false)
                _authenticationClients.Add(client.ProviderName, clientData);
        }

        public static void ClearClient()
        {
            _authenticationClients.Clear();
        }
    }

    public interface ICustomOAuthProvider
    {
        ICollection<AuthenticationClientData> RegisteredClientData { get; }

        AuthenticationResult VerifyOAuthAuthentication(string returnUrl);

        bool OAuthLogin(string provider, string providerUserId, bool persistCookie);

        AuthenticationClientData GetOAuthClientData(string providerName);

        string GetUserNameFromOpenAuth(string provider, string providerUserId);

        void RequestOAuthAuthentication(string provider, string returnUrl);
    }

    public class AspnetEnvironment : IApplicationEnvironment
    {
        public void IssueAuthTicket(string username, bool persist)
        {
            FormsAuthentication.SetAuthCookie(username, persist);
        }

        public void RevokeAuthTicket()
        {
            FormsAuthentication.SignOut();
        }

        public HttpContextBase AcquireContext()
        {
            return new HttpContextWrapper(HttpContext.Current);
        }

        public void RequestAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider, string returnUrl)
        {
            var securityManager = new OpenAuthSecurityManager(new HttpContextWrapper(HttpContext.Current), client, provider);

            securityManager.RequestAuthentication(returnUrl);
        }

        public string GetOAuthPoviderName()
        {
            var context = new HttpContextWrapper(HttpContext.Current);

            return OpenAuthSecurityManager.GetProviderName(context);
        }

        public AuthenticationResult VerifyAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider, string returnUrl)
        {
            var context = new HttpContextWrapper(HttpContext.Current);

            var securityManager = new OpenAuthSecurityManager(context, client, provider);

            return securityManager.VerifyAuthentication(returnUrl);
        }

    }

    public interface IApplicationEnvironment
    {
        void IssueAuthTicket(string username, bool persist);

        void RevokeAuthTicket();

        string GetOAuthPoviderName();

        void RequestAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider, string returnUrl);

        AuthenticationResult VerifyAuthentication(IAuthenticationClient client, IOpenAuthDataProvider provider, string returnUrl);

        HttpContextBase AcquireContext();
    }

    public class SecurityEncoder : ISecurityEncoder
    {
        public string Encode(string plainText, string salt)
        {
            return EncodePassword(plainText, salt);
        }

        public string SerializeOAuthProviderUserId(string providerName, string providerUserId)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(providerName);
                bw.Write(providerUserId);
                bw.Flush();
                var serializedWithPadding = new byte[ms.Length + _padding.Length];
                Buffer.BlockCopy(_padding, 0, serializedWithPadding, 0, _padding.Length);
                Buffer.BlockCopy(ms.GetBuffer(), 0, serializedWithPadding, _padding.Length, (int)ms.Length);
                return MachineKey.Encode(serializedWithPadding, MachineKeyProtection.All);
            }
        }

        public bool TryDeserializeOAuthProviderUserId(string protectedData, out string providerName, out string providerUserId)
        {
            providerName = null;
            providerUserId = null;
            if (String.IsNullOrEmpty(protectedData))
            {
                return false;
            }

            var decodedWithPadding = MachineKey.Decode(protectedData, MachineKeyProtection.All);

            if (decodedWithPadding.Length < _padding.Length)
            {
                return false;
            }

            // timing attacks aren't really applicable to this, so we just do the simple check.
            for (var i = 0; i < _padding.Length; i++)
            {
                if (_padding[i] != decodedWithPadding[i])
                {
                    return false;
                }
            }

            using (var ms = new MemoryStream(decodedWithPadding, _padding.Length, decodedWithPadding.Length - _padding.Length))
            using (var br = new BinaryReader(ms))
            {
                try
                {
                    // use temp variable to keep both out parameters consistent and only set them when the input stream is read completely
                    var name = br.ReadString();
                    var userId = br.ReadString();
                    // make sure that we consume the entire input stream
                    if (ms.ReadByte() == -1)
                    {
                        providerName = name;
                        providerUserId = userId;
                        return true;
                    }
                }
                catch
                {
                    // Any exceptions will result in this method returning false.
                }
            }
            return false;
        }

        public string GenerateSalt()
        {
            var buffer = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        private string EncodePassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            var hashStrategy = HashAlgorithm.Create("HMACSHA256") as KeyedHashAlgorithm;
            if (hashStrategy.Key.Length == saltBytes.Length)
            {
                hashStrategy.Key = saltBytes;
            }
            else if (hashStrategy.Key.Length < saltBytes.Length)
            {
                var keyBytes = new byte[hashStrategy.Key.Length];
                Buffer.BlockCopy(saltBytes, 0, keyBytes, 0, keyBytes.Length);
                hashStrategy.Key = keyBytes;
            }
            else
            {
                var keyBytes = new byte[hashStrategy.Key.Length];
                for (var i = 0; i < keyBytes.Length; )
                {
                    var len = Math.Min(saltBytes.Length, keyBytes.Length - i);
                    Buffer.BlockCopy(saltBytes, 0, keyBytes, i, len);
                    i += len;
                }
                hashStrategy.Key = keyBytes;
            }
            var result = hashStrategy.ComputeHash(passwordBytes);
            return Convert.ToBase64String(result);
        }

        private static readonly byte[] _padding = new byte[] { 0x85, 0xC5, 0x65, 0x72 };
    }

    public interface ISecurityEncoder
    {
        string GenerateSalt();

        string Encode(string plainText, string salt);

        string SerializeOAuthProviderUserId(string providerName, string providerUserId);

        bool TryDeserializeOAuthProviderUserId(string protectedData, out string providerName, out string providerUserId);
    }
}