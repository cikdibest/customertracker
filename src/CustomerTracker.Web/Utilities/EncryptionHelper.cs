using System.Configuration;
using System.Linq;
using System.Web;
using CustomerTracker.Common;
using CustomerTracker.Web.App_Start;
using JetBrains.Annotations;
using Ninject;

namespace CustomerTracker.Web.Utilities
{
    //public static class EncryptExtensions
    //{
    //    private static readonly IEncrypt _encrypt;

    //    static EncryptExtensions()
    //    {
    //        _encrypt = NinjectWebCommon.GetKernel.Get<IEncrypt>();
    //    }

    //    public static string Encrypt(this string text)
    //    {
    //        return _encrypt.Encrypt(text);
    //    }

    //    public static string Decrypt(this string text)
    //    {
    //        return _encrypt.Decrypt(text);
    //    }
    //}

}
