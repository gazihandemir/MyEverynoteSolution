using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEverynote.WebApp.Models;
using MyEverynote.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEverynote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
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

            // Değiştirilme zamanına göre bütün notların listelenmesi
            return View(noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList()); // c# tarafına bırakıyoruz 
                                                                                                                                   // return View(nm.getAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList()); // veritabanına bırakıyoruz 
        }
        public ActionResult ByCategory(int? id)
        {
            // Kategorideki id yoksa hata döndür.
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Category id bulma
       //    Category cat = categoryManager.Find(x => x.Id == id.Value);
            // id yoksa hata döndür
      /*     if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            } */
            // Kategorileri değiştirilme tarihine göre sırala 
           // List<Note> notes = cat.Notes.Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
           List<Note> notes = noteManager.ListQueryable().
                Where(x => x.IsDraft == false && x.CategoryId == id).
                OrderByDescending(x => x.ModifiedOn).ToList();

            return View("Index", notes);
           // return View("Index", cat.Notes.Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            /*
                 CategoryManager cm = new CategoryManager();  // cshtml sayfasının içinde bulunan c# kodları
                List<Category> list = cm.getCategories();
             */

            // En Beğenilenler'e tıklanıldıgında notlar begeni sayısına göre sıralansın
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult ShowProfile()
        {

            //   EvernoteUser currentUser = Session["login"] as EvernoteUser;

            // Kullanıcının id'sini fonksiyon yardımı ile buluyoruz.
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);
            // eğer hata var ise 
            if (res.Errors.Count > 0)
            {
                // TODO : Kullanıcıyı bir hata ekranına yönlendirmek gerekiyor..
                ErrorViewModel ErrorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                // Hata var ise hata ekranına yönlendir.
                return View("Error", ErrorNotifyObj);
            }
            // Eğer yok ise ShowProfile sayfasını aç ve sonucu yolla
            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            // EvernoteUser currentUser = Session["login"] as EvernoteUser;

            // Kullanıcının id'sinden kullanıcıyı buluyoruz.
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                // TODO : Kullanıcıyı bir hata ekranına yönlendirmek gerekiyor..
                ErrorViewModel ErrorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", ErrorNotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername"); //  ModifiedUsername Yoktur hatasını gizlemek için 
            // Eğer model var ise
            if (ModelState.IsValid)
            {
                // Eğer kullanıcının yüklemek istediği profil fotoğrafı istedigimiz formatta ise 
                if (ProfileImage != null && (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"))
                {
                    // Dosya adını tanımlıyoruz
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}"; // ör: user_12.png 
                    // Profil fotoğrafının gözükmesini sağlıyoruz.
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    // Kullanıcının profil resmi ismini veritabanına kaydediyoruz.
                    model.ProfileImageFilename = filename;
                }

                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.UpdateProfile(model);
                // Eğer hata var ise 
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    // Hata mesajı sayfasına yönlendir
                    return View("Error", errorNotifyObj);
                }

                //    Session["login"] = res.Result; 
                // Profil güncellendiği için session güncellendi.
                CurrentSession.Set<EvernoteUser>("login", res.Result);
                // Eğer işlem başarılı ise ShowProfile sayfasına git
                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }
        public ActionResult DeleteProfile()
        { // Profil silme
          // EvernoteUser currentUser = Session["login"] as EvernoteUser;

            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RemoveUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
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

                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.LoginUser(model);
                // Eğer hata varsa ife gir 
                if (res.Errors.Count > 0)
                {
                    // res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    // hata var ise ekrana hataları gönder ve ekranı tekrardan başlat
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    // sayfayı yüklee

                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "E-Posta Gönder";
                    }
                    return View(model);
                }

                //   Session["login"] = res.Result;          // Session'a kullanıcı bilgi saklama..
                CurrentSession.Set<EvernoteUser>("login", res.Result);
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


                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RegisterUser(model);
                if (res.Errors.Count > 0)
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

                //  return RedirectToAction("RegisterOk");
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz." +
                    "Hesabınızı aktive etmeden önce not ekleyemez ve begenme yapamazsınız.");
                return View("Ok", notifyObj);
            }

            return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            // Kullanıcı aktivasyonu saglanacak 

            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                TempData["errors"] = res.Errors;
                // return RedirectToAction("UserActivateCancel");
                return View("Error", ErrorNotifyObj);
            }
            OkViewModel OkNotifyOnj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login",
            };
            OkNotifyOnj.Items.Add(" Hesabınız aktifleştirildi.Artık not paylaşabilir ve beğenme yapabilirsiniz.");
            // return RedirectToAction("UserActivateOk");
            return View("Ok", OkNotifyOnj);



        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }
        /*  public ActionResult RegisterOk()
       {
           return View();
       }      
       public ActionResult UserActivateOk()
        {
            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
               errors = TempData["errors"] as List<ErrorMessageObj>;
            }    

            return View(errors);
        }
       
         */
        #region TestNotify
        /*  public ActionResult TestNotify()
        {
            /*   OkViewModel model = new OkViewModel()
            //InfoViewModel model = new InfoViewModel()
            //WarningViewModel model = new WarningViewModel()
               {
                   Header = "Yönlendirme",
                   Title="ok test",
                   RedirectingTimeout=3000,
                   Items=new List<string>() { "test basarılı 1","test basarılı 2"}
               }; (*)/
        ErrorViewModel model = new ErrorViewModel()
            {
                Header = "Yönlendirme",
                Title = "error test",
                RedirectingTimeout = 3000,
                Items = new List<ErrorMessageObj>() { 
                  new ErrorMessageObj() { Message="test 1 başarılı"},
                  new ErrorMessageObj() { Message="test 2 başarılı"}
               }
            };
            return View("Error",model);
        }
    */
        #endregion
    }

}