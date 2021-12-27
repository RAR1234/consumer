using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRAWebApplication.ConfigSetting;

namespace TRAWebApplication.Authorization
{
    public class AuthCredential
    {
        public static bool Login(string username,string password)
        {
            ConfigData _serviceConfiguration = ConfigEncrypt.GetCrmCredentials();
            if (_serviceConfiguration != null)
            {
                string usernames = Encryption.Auth_Decrypt(_serviceConfiguration.Auth_UserName);
                string passwords = Encryption.Auth_Decrypt(_serviceConfiguration.Auth_Password);

                if (username == usernames && password == passwords)
                {
                    return true;
                }else
                {
                    return false;
                }
            }
            return false;
        }
    }
}