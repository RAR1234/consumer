using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using TRAWebApplication.ConfigSetting;
using TRAWebApplication.Controllers;
using TRAWebApplication.Models;

namespace TRAWebApplication.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        public static string _returnUrl="";

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }
      
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            consumerSuccess consumerLogins = new consumerSuccess();
            consumerLogin _loginVM = null;

            try
            {
                if (context != null)
                {

                    consumerLogins.Identity = new ClaimsIdentity(context.Options.AuthenticationType);


                    OrganizationService organization = new OrganizationService();

                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    IOrganizationService service = organization.GetCRMService(configData);

                    if (service != null)
                    {
                        string fetchDocuments = string.Format(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                           <entity name='contact'>
                                            <attribute name='fullname' />
                                            <attribute name='telephone1' />
                                            <attribute name='firstname' />
                                            <attribute name='contactid' />
                                            <attribute name='tra_password' />
                                            <attribute name='emailaddress1' />
                                            <order attribute='fullname' descending='false' />
                                            <filter type='and'>
                                              <filter type='or'>
                                                <filter type='and'>
                                                  <condition attribute='emailaddress1' operator='eq' value='{0}' />
                                                  <condition attribute='tra_password' operator='eq' value='{1}' />
                                                </filter>
                                                <condition attribute='emailaddress1' operator='eq' value='{0}' />
                                              </filter>
                                            </filter>
                                          </entity>
                                        </fetch>", context.UserName,context.Password);


                        EntityCollection _loginuserEntity = service.RetrieveMultiple(new FetchExpression(fetchDocuments));

                        if (_loginuserEntity != null && _loginuserEntity.Entities.Count==1)
                        {


                            _loginVM = new consumerLogin();

                            _loginVM.emailId = _loginuserEntity.Entities[0].Contains("emailaddress1") ? (string)_loginuserEntity.Entities[0].Attributes["emailaddress1"] : string.Empty;
                            _loginVM.password = _loginuserEntity.Entities[0].Contains("tra_password") ? (string)_loginuserEntity.Entities[0].Attributes["tra_password"] : string.Empty;
                            _loginVM.fullname = _loginuserEntity.Entities[0].Contains("fullname") ? (string)_loginuserEntity.Entities[0].Attributes["fullname"] : string.Empty;
                            _loginVM.contactId = _loginuserEntity.Entities[0].Contains("contactid") ? (Guid)_loginuserEntity.Entities[0].Attributes["contactid"] : Guid.Empty;
                            _loginVM.firstname = _loginuserEntity.Entities[0].Contains("firstname") ? (string)_loginuserEntity.Entities[0].Attributes["firstname"] : string.Empty;

                            //Changes By Shweta
                            String LoginPassword = Encryption.Decrypt(_loginVM.password);
                            if (_loginuserEntity.Entities.Count == 1 && context.Password.Trim() != LoginPassword)//Changes
                            {
                                context.SetError("invalid_grant", "Please enter valid login credentials");
                            }
                            else
                            if(_loginuserEntity.Entities.Count == 1&& context.UserName.Trim()==_loginVM.emailId&&context.Password.Trim() == LoginPassword)//Changes
                            { 
                                consumerLogins.contactId = _loginVM.contactId;
                                consumerLogins.fullname = _loginVM.fullname;
                                consumerLogins.errorCode = HttpStatusCode.OK;
                                consumerLogins.firstname = _loginVM.firstname;


                                consumerLogins.Identity.AddClaim(new Claim(System.Security.Claims.ClaimTypes.Role, "Users"));

                                consumerLogins.Identity.AddClaim(new Claim(System.Security.Claims.ClaimTypes.Name, context.UserName));
                                consumerLogins.Identity.AddClaim(new Claim("fullname", consumerLogins.fullname));
                                consumerLogins.Identity.AddClaim(new Claim("firstname", consumerLogins.firstname));

                                consumerLogins.Identity.AddClaim(new Claim("contactId", Convert.ToString(consumerLogins.contactId)));
                                consumerLogins.Identity.AddClaim(new Claim("errorCode", "200"));
                                consumerLogins.Identity.AddClaim(new Claim("Loggedon", DateTime.Now.ToString()));

                                context.Validated(consumerLogins.Identity);

                            }
                            

                        }
                        else if(_loginuserEntity.Entities.Count==0)
                        {
                            context.SetError("invalid_grant", "An account with the username was not found. Please enter valid login details.");
                           
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
           
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            
                if (context.ClientId == null)
                {
                    context.Validated();
                }

                return Task.FromResult<object>(null);
            
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            try
            {
                AccountController.returnUrl = context.RedirectUri;
               
              
                if (context.ClientId == _publicClientId)
                {
                    context.Validated();
                }

                //Uri expectedRootUri = new Uri(context.Request.Uri,"Ar-Bh/Consumer/index.html?isLogin=true/");
                //        if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                //        {
                //            context.Validated();
                //        }
                //    }
                //    else if (context.RedirectUri.Contains("/en-US/Consumer/index.html?isLogin=true/"))
                //    {
                //        Uri expectedRootUri = new Uri(context.Request.Uri, "/en-US/Consumer/index.html?isLogin=true/");
                //        if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                //        {
                //            context.Validated();
                //        }
                //    }



                //}
            }
            catch(Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}