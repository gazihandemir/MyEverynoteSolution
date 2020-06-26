using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEverynote.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
  /*      public ActionResult Select(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CategoryManager cm = new CategoryManager();
            Category cat = cm.getCategoryById(id.Value);
            if(cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }
            TempData["mm"] = cat.Notes;
            return RedirectToAction("Index", "Home");
        }
        */
    }
}