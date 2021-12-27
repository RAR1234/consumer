using System;
using System.Collections.Generic;

namespace TRAWebApplication.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }
    public class userModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
        public string fullname { get; set; }
    }
    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public string providerKey { get; set; }
        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
        public Guid contactId { get; set; }

        public string fullname { get; set; }
        public string firstname { get; set; }
    }
    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }   
}
