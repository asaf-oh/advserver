using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace advertiser
{
	public class AccountController : Controller
	{
		public ActionResult Index ()
		{
			/*ViewData["Message"] = "welcome Shimon";
			ViewData.Model = new Account(-1, "", ""); 
			return View ();*/
			return Login();
		}

		public ActionResult Login ()
		{
			ViewData["Message"] = "Login";
			ViewData.Model = new Account(-1, "", ""); 
			return View ();
		}

		public ActionResult AddStart ()
		{
			
			return View();
		}
		
		public ActionResult AddFinish(Account acc)
		{
			Console.WriteLine("AddFinish : " + acc.ToString());
			ViewData["Success"] = true;
			return View();
		}
		
		public ActionResult LoginEnd ()
		{
			ViewData.Model = new Account(-1, "xxx","");
			return View();
		}
	}
}

