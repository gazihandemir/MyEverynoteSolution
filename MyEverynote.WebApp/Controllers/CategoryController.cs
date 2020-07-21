using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.Entities;

namespace MyEverynote.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        // Kategori için ayarlanan Controller
        // Sürekli kullanacağımız için en üst tarafta CategoryManager nesnesi oluşturuyoruz.
        private CategoryManager categoryManager = new CategoryManager();
        public ActionResult Index()
        {   // Index Sayfasındaki categorilerin ve özelliklerinin listelenmesi 
            return View(categoryManager.List()); 
        }

      
        public ActionResult Details(int? id)
        {
            // Kategorilerin Details Actionunda Tıklanan kategorinin detayına(id) girmemiz 
            // Ve özelliklerinin sıralanması
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
        { // Create Sayfasının açılması
            return View();
        }

    
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            // Kategori oluştururken üstteki değerler otomatik olarak ayarlanıyor.
            // Fakat sisteme göre hata döndürdügüne için bunları siliyoruz
            if (ModelState.IsValid) 
            {
                categoryManager.Insert(category); // Kategoriyi oluştur
                
                return RedirectToAction("Index"); // oluşturduktan sonra index sayfasına git
            }

            return View(category); 
        }

      
        public ActionResult Edit(int? id)
        { // Edit sayfasının kategori id'sina göre açılması
           
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
        public ActionResult Edit(Category category)
        {
            // Kategori için edit sayfası
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            // Kategori oluştururken üstteki değerler otomatik olarak ayarlanıyor.
            // Fakat sisteme göre hata döndürdügüne için bunları siliyoruz
            if (ModelState.IsValid)
            {
                Category cat = categoryManager.Find(x => x.Id == category.Id); // kategoriyi buluyoruz
                cat.Title = category.Title; // başlıgını güncelliyoruz
                cat.Description = category.Description;  // açıklamasını güncelliyoruz 
                categoryManager.Update(cat); // Günlliyoruz
                return RedirectToAction("Index");
            }
            return View(category);
        }

   
        public ActionResult Delete(int? id)
        { // Silme sayfasının kategorinin ıd'sine göre açılması
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
        {// Kategori bulunup silinir.
            Category category = categoryManager.Find(x => x.Id == id);
            categoryManager.Delete(category);
            return RedirectToAction("Index");
        }

    }
}
