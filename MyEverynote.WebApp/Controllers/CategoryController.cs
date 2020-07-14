using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.Entities;

namespace MyEverynote.WebApp.Controllers
{
    public class CategoryController : Controller
    { 

        private CategoryManager categoryManager = new CategoryManager();
        public ActionResult Index()
        {
            return View(categoryManager.List()); 
        }

      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

    
        public ActionResult Create()
        {
            return View();
        }

    
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,CreatedOn,ModifiedOn,ModifiedUserName")] Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                categoryManager.Insert(category);
                
                return RedirectToAction("Index");
            }

            return View(category);
        }

      
        public ActionResult Edit(int? id)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

 
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,CreatedOn,ModifiedOn,ModifiedUserName")] Category category)
        {
            if (ModelState.IsValid)
            {
                Category cat = categoryManager.Find(x => x.Id == category.Id);
                cat.Title = category.Title;
                cat.Description = category.Description; 
                categoryManager.Update(cat);
                return RedirectToAction("Index");
            }
            return View(category);
        }

   
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryManager.Find(x => x.Id == id);
            categoryManager.Delete(category);
            return RedirectToAction("Index");
        }

    }
}
