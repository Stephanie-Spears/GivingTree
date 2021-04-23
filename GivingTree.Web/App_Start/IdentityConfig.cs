﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

using GivingTree.Web.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

using SendGrid;
using SendGrid.Helpers.Mail;

using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace GivingTree.Web
{
	public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            await ConfigSendGridasync(message);
        }
        private static async Task ConfigSendGridasync(IdentityMessage message)
        {
	        string apiKey = ConfigurationManager.AppSettings["SendGridKey"];
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress(ConfigurationManager.AppSettings["mailAccountGmail"], "The Giving Tree via SendGrid"),
                Subject = message.Subject,
                HtmlContent = message.Body
			};

			msg.AddTo(new EmailAddress(message.Destination, "New Giving Tree User"));

			await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
	}

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
	        string accountSid = ConfigurationManager.AppSettings["SMSAccountIdentificationLIVE"];
	        string authToken = ConfigurationManager.AppSettings["SMSAccountAuthTokenLIVE"];
	        string fromNumber = ConfigurationManager.AppSettings["SMSAccountFromLIVE"];

			TwilioClient.Init(accountSid, authToken);

			MessageResource result = MessageResource.Create(
			new PhoneNumber(message.Destination),
			@from: new PhoneNumber(fromNumber),
			body: message.Body
			);

			//Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
			Trace.TraceInformation(result.Status.ToString());
			//Twilio doesn't currently have an async API, so return success.
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

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
	            RequireNonLetterOrDigit = true,
               RequireDigit = true,
               RequireLowercase = true,
               RequireUppercase = true,
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
