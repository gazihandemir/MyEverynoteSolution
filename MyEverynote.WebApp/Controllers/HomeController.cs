using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.ValueObjects;
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
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid) // Veriler girildiyse ife gir 
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> res = eum.LoginUser(model);
                // Eğer hata varsa ife gir 
                if (res.Errors.Count > 0)
                {
                    // res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    // hata var ise ekrana hataları gönder ve ekranı tekrardan başlat
                    res.Errors.ForEach(x => ModelState.AddModelError("",))
                    // sayfayı yüklee

                    if ()

                    return View(model);
                }

                Session["login"] = res.Result;          // Session'a kullanıcı bilgi saklama..
                return RedirectToAction("Index");       // Yönlendirme



            }
            return View(model);
          
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            // Kullanıcı username kontrolü
            // Kullanıcı eposta kontrolü
            // Kayıt işlemi
            // Aktivasyon e postası gönderimi
            if (ModelState.IsValid)
            {
                EvernoteUserManager eum = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> res = eum.RegisterUser(model);
                if(res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }

                /*   EvernoteUser user = null;
                   try {
                       eum.RegisterUser(model);
                   }
                   catch(Exception ex)
                   {
                       ModelState.AddModelError("", ex.Message);
                   }



                /*   if(model.Username == "aaa")
                   {
                       ModelState.AddModelError("", "Kullanıcı adı kullanılıyor.");
                   }
                   if(model.Email == "aaa@aa.com")
                   {
                       ModelState.AddModelError("", "e-mail  kullanılıyor.");
                   }
                   foreach (var item in ModelState)
                   {
                       if(item.Value.Errors.Count > 0)
                       {
                           return View(model);
                       }
                   }
                   if(user == null)
                   {
                       return View(model);
                   }
                   */
                return RedirectToAction("RegisterOk");

            }
         
            return View(model);
        }
        public ActionResult RegisterOk()
        {
            return View();
        }
        public ActionResult UserActivate(Guid activate_id)
        {
            // Kullanıcı aktivasyonu saglanacak 

            return View();
        }
        public ActionResult Logout()
        {
            return View();
        }

    }
}