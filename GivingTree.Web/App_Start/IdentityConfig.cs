using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using GivingTree.Web.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GivingTree.Web
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            await configSendGridasync(message);
        }
        private async Task configSendGridasync(IdentityMessage message)
        {
			// From SendGrid Docs
			/*			string apiKey = ConfigurationManager.AppSettings["SendGridKey"];
						var client = new SendGridClient(apiKey);
						var from = new EmailAddress(ConfigurationManager.AppSettings["mailAccountGmail11"], "The Giving Tree via SendGrid");
						string subject = "The Giving Tree - Confirm Your Account";
						
						var to = new EmailAddress(ConfigurationManager.AppSettings["mailAccountGmail"], "test user");
						string plainTextContent = "Account Confirmation for The Giving Tree - Complete your registration";
						string htmlContent = "<strong>Insert Account Confirmation HTML and Links Here</strong>";
						var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
						var response = await client.SendEmailAsync(msg).ConfigureAwait(false);*/


			//string apiKey = ConfigurationManager.AppSettings["SendGridKey"];
			//var client = new SendGridClient(apiKey);
			//var msg = new SendGridMessage();
			//         msg.SetSubject("Thank you for signing up, % name %");



			// For more advanced cases, we can build the SendGridMessage object ourselves with these minimum required settings

			string apiKey = ConfigurationManager.AppSettings["SendGridKey"];
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				// todo: configure variable substitution to enable dynamic user details in account confirmation email
				From = new EmailAddress(ConfigurationManager.AppSettings["mailAccountGmail11"], "The Giving Tree via SendGrid"),
				Subject = "Account Confirmation for The Giving Tree - Complete Your Registration",
				PlainTextContent = "Finish Setting Up Your Account for The Giving Tree",
				HtmlContent = "<p>Follow the link below to confirm your account for" +
				              "<strong>" +
				              "The Giving Tree" +
                              "</strong>" +
				              "</p>" +
				              "</hr>" +
				              "<a href=''>Confirm Email</a>"
			};
			/* TODO: have email be created dynamically with user email and username */
			msg.AddTo(new EmailAddress(ConfigurationManager.AppSettings["mailAccountGmail"], "New User"));

			var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
	}

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

			/* TODO: turn password validators back on when done testing */
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
               // RequireNonLetterOrDigit = true,
               // RequireDigit = true,
               // RequireLowercase = true,
               // RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
