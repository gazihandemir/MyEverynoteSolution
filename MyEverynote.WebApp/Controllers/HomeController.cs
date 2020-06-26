using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
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
            // MyEvernote.BusinessLayer.test test = new MyEvernote.BusinessLayer.test();
            //test.InsertTest();
            // test.UpdateTest();
            // test.DeleteTest();
            //test.commentTest();
            /*  if(TempData["mm"] != null)
              {
                  return View(TempData["mm"] as List<Note>);
              }*/
            NoteManager nm = new NoteManager();
            return View(nm.getAllNote().OrderByDescending(x => x.ModifiedOn).ToList()); // c# tarafına bırakıyoruz 
          // return View(nm.getAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList()); // veritabanına bırakıyoruz 
        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CategoryManager cm = new CategoryManager();
            Category cat = cm.getCategoryById(id.Value);
            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }
            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();

            return View("Index",nm.getAllNote().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }
    }
}