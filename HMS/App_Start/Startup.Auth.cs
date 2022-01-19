using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using HMS.Models;
using HMS.Data;
using HMS.Entities;
using HMS.Services;

namespace HMS
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            
            app.CreatePerOwinContext(HMSContext.Create);
            app.CreatePerOwinContext<HMSUserManager>(HMSUserManager.Create);
            app.CreatePerOwinContext<HMSSignInManager>(HMSSignInManager.Create);
            app.CreatePerOwinContext<HMSRoleManager>(HMSRoleManager.Create);

           
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<HMSUserManager, HMSUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

           
        }
    }
}