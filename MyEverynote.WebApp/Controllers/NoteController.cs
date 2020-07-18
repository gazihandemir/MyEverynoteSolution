using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEverynote.WebApp.Models;

namespace MyEverynote.WebApp.Controllers
{
    public class NoteController : Controller
    {
        NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();

        public ActionResult Index()
        {
            var notes = noteManager.ListQueryable().Include("Category")
                .Where(x => x.Owner.Id == CurrentSession.User.Id)
                .OrderByDescending(x => x.ModifiedOn);
            return View(notes.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }


        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            if (ModelState.IsValid)
            {

                noteManager.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = noteManager.Find(x => x.Id == id);
            noteManager.Delete(note);

            return RedirectToAction("Index");
        }

    }
}
