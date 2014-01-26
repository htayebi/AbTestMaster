AbTestMaster
============

AbTestMaster is a free split testing framework for ASP.NET MVC applications. It makes performing A/B testing extremely simple.

How to Install
--------------
The easiest way to add ABTestMaster to your application is to install [Nuget Package] (https://www.nuget.org/packages/AbTestMaster/) via VIsual Studio. In Package Manager Console, type:

        Install-Package AbTestMaster
        
Alternatively, right click on your MVC project, click on Manage Nuget Packages and search for AbTestMaster.

Minimum Requirements
--------------------
- .NET Framework 4.0 or higher
It has two dependencies which get installed with the Nuget package:
- ASP.NET MVC 3 (or higher)
- Web Activator 1.5.3


How to Use
----------
Let's say you have two versions of your home page and would like to perform A/B testing on them. That's what you would need to do:

        public class HomeController : AbTestMasterController
        {
          [SplitView("Version 1", "HomeTest")]
          public ActionResult Index()
          {
            return View();
          }
          
          [SplitView("Version 2", "HomeTest")]
          public ActionResult Index2()
          {
            return View();          
          }
        }
        
Things to notice in the code sample above:
- Controller inherits from AbTestMasterController.
- SplitView attribute is applied to Index and Index2 action methods. The first parameter is the name of this specific view. The second parameter (called SplitGroup) is how you tell the framework that these views are part of the same test.

When a user visits Home/Index, AbTestMaster Framework notices the SplitView attribute and looks for any other attribute with the same SplitGroup. It randomly picks one of them and displays it to the user.

You will notice that when installing the Nuget package, a couple of CSV files are added to "App_Data" folder. One of them is AB_TEST_MASTER_SplitViews.csv. This is where you can see a log of split views showed to user.

Things to watch out for:
- Once a user visits one of the split views, a cookie is saved in their browser so they will see that version all the time.
- While there is nothing stopping you from having the same split groups in multiple controllers or even areas, that is not recommended as it makes it difficult for you to track which action methods are bundled in the same test.
- Once any of the split view action methods are called, a record is added to the CSV file (mentioned above). That is only when the result of calling the action is successful and HTTP response code is between 200 to 207. This means, if there is an Authorize attribute on the action and it fails, or action method returns a RedirectToAction result, it will not be logged.


More Features
-------------
AbTestMaster also enables you to define goals. This will help identify which views were displayed to the user before they were converted. Here's an example:

        public class PurchaseController : AbTestMasterController
        {
          [SplitView("Version 1", "ShoppingCart", "Checkout")]
          public ActionResult Cart()
          {
            return View();
          }
          
          [SplitView("Version 2", "ShoppingCart", "Checkout")]
          public ActionResult Cart2()
          {
            return View();
          }
          
          [SplitView("Version 1", "PaymentPage", "Checkout")]
          public ActionResult Payment()
          {
            return View();
          }
          
          [SplitView("Version 2", "PaymentPage", "Checkout")]
          public ActionResult Payment()
          {
            return View();
          }
          
          [SplitGoal("Checkout")]
          [HttpPost]
          public ActionResult Confirm()
          {
            return View();
          }
        }
        
In the sample code above, there are two views before Confirm page (our goal, where we consider customer as "converted"), Cart and Payment, each with two variations. You will notice that SplitView attribute is now called with another consutructor. The first two parameters are the view name and the split group as before. The third parameter is called sequence. In the example above, all SplitViews have the same sequence. When Confirm action method is called, ABTestMaster will look for all the SplitViews already called in the same sequence ("checkout" in the example above) and saves them in the AB_TEST_MASTER_SplitGOlas.csv.
