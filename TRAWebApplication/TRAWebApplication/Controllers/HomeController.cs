using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TRAWebApplication.Controllers
{
    public class HomeController : Controller
    {
       //[OutputCache(Duration =3)]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
           
            return Redirect("/en-us/consumer/index.html");
        }

        public ActionResult testpage1()
        {
            TempData["one"] = "Test page 1";
            return View();
        }

        public ActionResult testpage2()
        {
            TempData["one"] = TempData["one"] + "inside test page 2";
            return View();
        }

    }
}
