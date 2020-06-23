using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEverynote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            MyEvernote.BusinessLayer.test test = new MyEvernote.BusinessLayer.test();

            return View();
        }
    }
}