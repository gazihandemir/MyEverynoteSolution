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
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            // Kullanıcı username kontrolü
            // Kullanıcı eposta kontrolü
            // Kayıt işlemi
            // Aktivasyon e postası gönderimi
            EvernoteUser user =  repo_user.Find(x => x.UserName == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            if (user != null)
            {
              if(user.UserName == data.Username)
                {
                    //  layerResult.Errors.Add("Kullanıcı Adı kayıtlı.");
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if(user.Email == data.Email)
                {
                    // layerResult.Errors.Add("E-Posta adresi kayıtlı.");
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }
            else
            {
               int dbResult =  repo_user.Insert(new EvernoteUser()
                {
                    UserName = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                 
                    IsActive = false,
                    IsAdmin = false,


               });
                if(dbResult > 0)
                {
                    layerResult.Result = repo_user.Find(x => x.Email == data.Email && x.UserName == data.Username);
                    // TODO : activasyon maili atılacak.
                    // layerResult.Result.ActivateGuid
                }
            }
            return layerResult;
        }
   
        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            // Giriş kontrolü
            // Hesap aktive edilmiş mi ?
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            layerResult.Result = repo_user.Find(x => x.UserName == data.Username && x.Password == data.Password);

            if (layerResult.Result != null)
            {
                if (!layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    layerResult.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");

                    //layerResult.Errors.Add("Kullanıcı aktifleştirilmemiştir. Lütfen e-posta adresinizi kontrol ediniz.");
                } 
            }
            else
            {
                layerResult.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı Adı yada şifre uyuşmuyor.");
               // layerResult.Errors.Add("Kullanıcı adı yada şifre uyuşmuyor.");
            }

            return layerResult;
        }
    }
}
