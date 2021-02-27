using GivingTree.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GivingTree.Web;

namespace GivingTree.Web.Controllers
{
	public class EmailController : Controller
	{
		private SendGridEmailService _sendGridEmailService;
		public EmailController()
		{
			_sendGridEmailService = new SendGridEmailService();
		}

		public ActionResult Send()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Send(EmailContract emailContract)
		{
			if (!ModelState.IsValid)
			{
				return View();
				//return View(model);
			}
            
			try
			{
				var response= _sendGridEmailService.Send(emailContract);
				ViewBag.Success = true;
				return View();
			}
			catch (Exception)
			{
				ViewBag.Success = false;
				return View();
			}
		}

	}
}