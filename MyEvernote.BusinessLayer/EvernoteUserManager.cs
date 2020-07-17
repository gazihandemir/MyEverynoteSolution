using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Common.Helpers;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class EvernoteUserManager : ManagerBase<EvernoteUser>
    {
        // Data AccessLayerdaki Repository<T(Generic class)> nesnemizi(EvernoteUser) oluşturuyoruz.
        //  private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        // Kullanıcı kayıt yapmak için yazılmış fonksiyon.
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            // RegisterViewModel username,email,sifre,resifre -> data.(PROP)
            /* 
                    Yapılacaklar
            Kullanıcı username kontrolü
            Kullanıcı eposta kontrolü
            Kayıt işlemi
            Aktivasyon e postası gönderimi 
            */
            // Kullanıcının yazdığı Username veya Email  data'nın içindeki yani veri tabanının içinde var mı diye kontrol yapıyoruz.
            // Eğer var ise user null olmuyor , yok ise null oluyor buna göre kontrol yapacağız.
            EvernoteUser user = Find(x => x.UserName == data.Username || x.Email == data.Email); // Find -> Repository fonksiyon.
            // Hata mesajları için nesne oluşturuyoruz 
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            if (user != null) // Eğer kullanıcı var ise 
            {
                // Kullanıcı adı veri tabanında mevcut ise yani kullanıcı daha önceden kayıt olmuş 
                //yada başka birinin kullanıcı adı ile kayıt olmaya çalışıyor olabilir.
                if (user.UserName == data.Username)
                {
                    //  layerResult.Errors.Add("Kullanıcı Adı kayıtlı.");
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı."); // Hata mesajımız.
                }
                // Email veri tabanında mevcut ise yani kullanıcı daha önceden kayıt olmuş 
                //yada başka birinin Email'i ile kayıt olmaya çalışıyor olabilir.
                if (user.Email == data.Email)
                {
                    // layerResult.Errors.Add("E-Posta adresi kayıtlı.");
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı."); // Hata mesajımız.
                }
            }
            // Kullanıcı yok -> ise register(kayıt olma) yapacağız 
            else
            {
                // Ekleme
                int dbResult = base.Insert(new EvernoteUser() // int dbresult tanımlamamızın sebebi eğer kayıt yapılırsa dbresult değeri 1 artıyor ve veri tabanına bakarak eklendiğini anlıyoruz.
                {
                    UserName = data.Username,
                    Email = data.Email,
                    ProfileImageFilename = "user.jpg",
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(), // Rastgele aktivasyon kodu gönderme 

                    IsActive = false, // aktivasyon kodunu onayladı mı ? 
                    IsAdmin = false, // Yönetici mi ? 


                });
                if (dbResult > 0) // Eğer eklenmiş ise 
                {
                    layerResult.Result = Find(x => x.Email == data.Email && x.UserName == data.Username);
                    // TODO : activasyon maili atılacak.
                    // layerResult.Result.ActivateGuid
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerResult.Result.UserName};<br><br> Kesabınızı aktifleştirmek için <a href =' {activateUri}' " +
                        $"target='_blank'>tıklayınız.</a>";
                    MailHelper.SendMail(body, layerResult.Result.Email, "MyEvernote Hesap Aktifleştirme", true);
                    // https://support.google.com/accounts/answer/6010255?hl=tr 
                }
            }
            return layerResult;
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            /* Yapılacaklar
            Giriş kontrolü
            Hesap aktive edilmiş mi ? */
            // Hata mesajları oluşturmak için yeni bir nesne oluşturuyoruz.
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            // Kullanıcı var mı diye Find işlemi yapıyoruz.(Kullanıcı adı ve şifre uyuşuyor mu ?)
            layerResult.Result = Find(x => x.UserName == data.Username && x.Password == data.Password);

            if (layerResult.Result != null) // Eğer kullanıcı var ise 
            {
                if (!layerResult.Result.IsActive) // Kullanıcı aktivasyon kodunu aktif etmemiş ise 
                {
                    // Hata mesajlarımız
                    layerResult.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    layerResult.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");
                    //layerResult.Errors.Add("Kullanıcı aktifleştirilmemiştir. Lütfen e-posta adresinizi kontrol ediniz."); // Kodun eski hali
                }
            }
            else // Kullanıcı yok ise 
            {
                // Hata mesajımız.
                layerResult.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı Adı yada şifre uyuşmuyor.");
                // layerResult.Errors.Add("Kullanıcı adı yada şifre uyuşmuyor.");
            }

            return layerResult;
        }     // Kullanıcı giriş yap 


        public BusinessLayerResult<EvernoteUser> RemoveUserById(int id) // Kullanıcıyı id'sine göre silme işlemi
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            // Kullanıcıc id'sini bulmaya çalışma
            EvernoteUser user = Find(x => x.Id == id);
            // eğer kullanıcı var ise 
            if (user != null)
            {
                // Delete işlemi başarısız ise
                if (Delete(user) /*Kullanıcı hem silinir hem döndürdüğü int değer kontol edilir */ == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            // Kullanıcı bulunamadı ise 
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser data)
        { // Kullanıcının userName'ni veya Emaili var mı bulmaya çalışıyoruz. 
            EvernoteUser db_user = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            // Eğer kullanıcı var ise  <- &&
            // && -> Kullanıcı kendi ismini değiştirmez ise yani başka şeyleri değiştirip kullanıcı adını değiştirmek istemiyorsa
            // Kontrolü yapıyoruz . Kullanıcı adını tekrar alıp kayıt işlemi yapıyoruz.
            if (db_user != null && db_user.Id != data.Id)
            {
                // Kullanıcı başka bir kullanıcının UserName'ini kaydetmeye çalışıyor ise 
                if (db_user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                // Kullanıcı başka bir kullanıcının Email'ini kaydetmeye çalışıyor ise 
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı");
                }
                return res;
            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email; // Yazılan emaili veri tabanında güncelle
            res.Result.Name = data.Name; // Yazılan ismi veri tabanında güncelle
            res.Result.Surname = data.Surname; // Yazılan soy ismi veri tabanında güncelle
            res.Result.Password = data.Password; // Yazılan şifreyi veri tabanında güncelle
            res.Result.UserName = data.UserName; // Yazılan Kullanıcı adını veri tabanında güncelle
            // fotoğrafı güncellemek istiyor ise 
            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename; // Profil resmini güncelle
            }
            if (base.Update(res.Result)/*Kullanıcı hem güncellenir hem döndürdüğü int değer kontol edilir */  == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenemedi.");
            }
            return res;
        } // Kullanıcı profil güncelleştirme

        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            // aradğımız guid veri tabanındaki guide eşitmi buluyoruz
            layerResult.Result = Find(x => x.ActivateGuid == activateId);
            // Eger yukardaki işlem başarılı ise 
            if (layerResult.Result != null)
            {      // Kullanıcı aktifse IsActive == true
                if (layerResult.Result.IsActive)
                {   // Hata mesajı oluşturup sonucu geri döndür
                    layerResult.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return layerResult;
                }
                // Kullanıcıyı güncelle 
                layerResult.Result.IsActive = true;
                Update(layerResult.Result);
            }
            // Eğer aktifleştirilecek bir kullanıcı bulunamadı ise 
            else
            {
                // Hata mesajı ekle ve sonucu geri döndür
                layerResult.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirecek kullanıcı bulunamadı.");
            }
            return layerResult;
        }// Kullanıcı e-posta aktifleştirme maili 

        // Kullanıcının id'sini bulmak -> Kullanıcının profilini göstermek için 
        // adına tıkladıgımız zaman id'sini bulmak istiyoruz ve buna göre işlem yapmak istiyoruz.
        public BusinessLayerResult<EvernoteUser> GetUserById(int id)
        {
            // Hata oluşturmak için res nesnesi oluşturuyoruz.
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            // kullanıcıc id'sini bulmak
            res.Result = Find(x => x.Id == id);
            // Eğer id bulunamamışsa hata mesajı oluştur
            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }
            // id yoksa hata döndür , id varsa idnin kendisini gönder
            return res;
        }

        // Method hiding -> new 
        public new BusinessLayerResult<EvernoteUser> Insert(EvernoteUser data)
        {


            EvernoteUser user = Find(x => x.UserName == data.UserName || x.Email == data.Email);

            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            layerResult.Result = data;
            if (user != null)
            {

                if (user.UserName == data.UserName)
                {

                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {

                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }

            else
            {
                layerResult.Result.ProfileImageFilename = "user.jpg";
                layerResult.Result.ActivateGuid = Guid.NewGuid();
                if (base.Insert(layerResult.Result) == 0)
                {
                    layerResult.AddError(ErrorMessageCode.UserCountNotInserted, "Kullanıcı eklenemedi");
                }

            }
            return layerResult;

        }

        public new BusinessLayerResult<EvernoteUser> Update(EvernoteUser data)
        {

            EvernoteUser db_user = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {

                if (db_user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı");
                }
                return res;
            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı Güncellenemedi.");
            }
            return res;
        }
    }
}
