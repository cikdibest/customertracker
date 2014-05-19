using System;

namespace CustomerTracker.Web.Business.AccountBusiness
{
    public class PasswordCreater : IPasswordCreater
    {
       

        public PasswordCreater()
        {
           
        }

        public string Create()
        {
            var password = Guid.NewGuid().ToString();

            return password.Replace("-", "").Substring(0, 6);
        }


    }

    internal interface IPasswordCreater
    {
        string Create();
    }


}