using System;
using NUnit.Framework;
using System.Web.Mvc;
using advertiser;

namespace advertiser
{
	[TestFixture()]
	public class AccountFixture
	{
		[Test()]
		public void Add_Account_Should_Add_Account()
		{
			Console.WriteLine("Enter AddAccount");			
			// Store
			AccountController controller = new AccountController();
			string name = "shimon";
			Account acc = new Account(10, name, "==+jjohoinmb687uiyjkglh");			
			ViewResult result = (ViewResult)controller.AddFinish(acc);
			
			// Retrieve
			//System.Web.Mvc.Result
			
			//ViewResult result = (System.Web.Mvc.ViewResult)controller.Retreive(3);
			//ActionResult result = controller.Retreive(acc.Id);
			/*acc = (Account)result.ViewData.Model;*/
						
			Assert.That((bool)result.ViewData["Success"] == true);
		}
		
		[Test()]
		public void EditAccount ()
		{
				//Account acc = new Account(10, "shimon", "==+jjohoinmb687uiyjkglh");
				Assert.AreNotEqual(10,20);
		}
		
		[Test()]
		public void RemoveAccount ()
		{
				//Account acc = new Account(10, "shimon", "==+jjohoinmb687uiyjkglh");
				Assert.AreEqual(20,20);
		}
	
	}
}

