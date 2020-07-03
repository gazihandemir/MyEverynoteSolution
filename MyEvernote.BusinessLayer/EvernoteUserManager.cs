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
    public class EvernoteUserManager
    {
        // Data AccessLayerdaki Repository<T(Generic class)> nesnemizi(EvernoteUser) oluşturuyoruz.
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
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
            EvernoteUser user =  repo_user.Find(x => x.UserName == data.Username || x.Email == data.Email); // Find -> Repository fonksiyon.
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
               int dbResult =  repo_user.Insert(new EvernoteUser() // int dbresult tanımlamamızın sebebi eğer kayıt yapılırsa dbresult değeri 1 artıyor ve veri tabanına bakarak eklendiğini anlıyoruz.
                {
                    UserName = data.Username, 
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(), // Rastgele aktivasyon kodu gönderme 
                 
                    IsActive = false, // aktivasyon kodunu onayladı mı ? 
                    IsAdmin = false, // Yönetici mi ? 


               });
                if(dbResult > 0) // Eğer eklenmiş ise 
                {
                    layerResult.Result = repo_user.Find(x => x.Email == data.Email && x.UserName == data.Username);
                    // TODO : activasyon maili atılacak.
                    // layerResult.Result.ActivateGuid
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerResult.Result.UserName};<br><br> Kesabınızı aktifleştirmek için <a href =' {activateUri}' " +
                        $"target='_blank'>tıklayınız.</a>";
                    MailHelper.SendMail(body, layerResult.Result.Email,"MyEvernote Hesap Aktifleştirme",true);
                    // https://support.google.com/accounts/answer/6010255?hl=tr 
                }
            }
            return layerResult;
        }

      

        // Kullanıcı giriş yap 
        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            /* Yapılacaklar
            Giriş kontrolü
            Hesap aktive edilmiş mi ? */
            // Hata mesajları oluşturmak için yeni bir nesne oluşturuyoruz.
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            // Kullanıcı var mı diye Find işlemi yapıyoruz.(Kullanıcı adı ve şifre uyuşuyor mu ?)
            layerResult.Result = repo_user.Find(x => x.UserName == data.Username && x.Password == data.Password);

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
        }
        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            layerResult.Result = repo_user.Find(x => x.ActivateGuid == activateId);
            if(layerResult.Result != null)
            {
                if (layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return layerResult;
                }
                layerResult.Result.IsActive = true;
                repo_user.Update(layerResult.Result);
            }
            else
            {
                layerResult.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirecek kullanıcı bulunamadı.");
            }
            return layerResult;
        }
        public BusinessLayerResult<EvernoteUser> getUserBuId(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = repo_user.Find(x => x.Id == id);
            if(res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }
            return res;
        }
    }
}
