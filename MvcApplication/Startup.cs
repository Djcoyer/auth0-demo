using Auth0.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Configuration;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(MvcApplication.Startup))]

namespace MvcApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Set Cookies as default authentication type
            string auth0Domain = ConfigurationManager.AppSettings["auth0:Domain"];
            string auth0ClientId = ConfigurationManager.AppSettings["auth0:ClientId"];
            string auth0ClientSecret = ConfigurationManager.AppSettings["auth0:ClientSecret"];

            // Enable the Cookie saver middleware to work around a bug in the OWIN implementation
            app.UseKentorOwinCookieSaver();

            // Set Cookies as default authentication type
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/Account/Login")
            });

            // Configure Auth0 authentication
            var options = new Auth0AuthenticationOptions()
            {
                Domain = auth0Domain,
                ClientId = auth0ClientId,
                ClientSecret = auth0ClientSecret,

                SaveAccessToken = true,
                SaveIdToken = true,
                SaveRefreshToken = true,

                // If you want to request an access_token to pass to an API, then replace the audience below to 
                // pass your API Identifier instead of the /userinfo endpoint
                Provider = new Auth0AuthenticationProvider()
                {
                    OnApplyRedirect = context =>
                    {
                        string userInfoAudience = $"https://{auth0Domain}/userinfo";
                        string redirectUri = context.RedirectUri + "&audience=" + WebUtility.UrlEncode(userInfoAudience);

                        context.Response.Redirect(redirectUri);
                    },
                    OnAuthenticated = context =>
                    {
                        // Get the user's roles
                        var rolesObject = context.User["https://schemas.quickstarts.com/roles"];
                        if (rolesObject != null)
                        {
                            string[] roles = rolesObject.ToObject<string[]>();
                            foreach (var role in roles)
                            {
                                context.Identity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, context.Connection));
                            }
                        }


                        return Task.FromResult(0);
                    }
                }
            };
            options.Scope.Add("offline_access");
            app.UseAuth0Authentication(options);

        }
    }
}
