using System;

using Microsoft.AspNet.Identity;

using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using TRAWebApplication.Providers;
using TRAWebApplication.Models;
using Microsoft.Owin.Security.Facebook;
using TRAWebApplication.Facebook;
using System.Configuration;

namespace TRAWebApplication
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            string FacebookAppId = ConfigurationManager.AppSettings["FbAppId"].ToString();
            string FacebookAppSecret = ConfigurationManager.AppSettings["fbAppsecret"].ToString();
            string googleClientId = ConfigurationManager.AppSettings["googleClientId"].ToString();
            string googleSecretKey = ConfigurationManager.AppSettings["googleSecretKey"].ToString();

            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next();
            });

            var facebookOptions = new FacebookAuthenticationOptions()
            {
              //  AppId = "3635869396432596",
                AppId= FacebookAppId,
               // AppSecret = "4378b6c97720e8c2cbf5c8c27d57c60a",
                AppSecret= FacebookAppSecret,
                BackchannelHttpHandler = new FacebookBackChannelHandler(),
                UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,email,name"
            };
            
            facebookOptions.Scope.Add("email");
            app.UseFacebookAuthentication(facebookOptions);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = googleClientId,
                ClientSecret = googleSecretKey

            });
        }
    }
}
