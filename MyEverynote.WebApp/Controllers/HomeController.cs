using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
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
            // Değiştirilme zamanına göre bütün notların listelenmesi
            return View(nm.getAllNote().OrderByDescending(x => x.ModifiedOn).ToList()); // c# tarafına bırakıyoruz 
          // return View(nm.getAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList()); // veritabanına bırakıyoruz 
        }
        public ActionResult ByCategory(int? id)
        {
            // Kategorideki id yoksa hata döndür.
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CategoryManager cm = new CategoryManager();
            // Category id bulma
            Category cat = cm.getCategoryById(id.Value);
            // id yoksa hata döndür
            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }
            // Kategorileri değiştirilme tarihine göre sırala 
            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            /*
                 CategoryManager cm = new CategoryManager();  // cshtml sayfasının içinde bulunan c# kodları
                List<Category> list = cm.getCategories();
             */
            NoteManager nm = new NoteManager();
            // En Beğenilenler'e tıklanıldıgında notlar begeni sayısına göre sıralansın
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
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    // sayfayı yüklee

                   if(res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "E-Posta Gönder";
                    }
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
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
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
            EvernoteUserManager eum = new EvernoteUserManager();
           BusinessLayerResult<EvernoteUser> res = eum.ActivateUser(activate_id);
            if(res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            } 

            // Kullanıcı aktivasyonu saglanacak 

            return View();
        } 
        public ActionResult UserActivateOk(Guid activate_id)
        {
            return View();
        }
        public ActionResult UserActivateCancel(Guid activate_id)
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
               errors = TempData["errors"] as List<ErrorMessageObj>;
            }    

            return View(errors);
        }
        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

    }
}