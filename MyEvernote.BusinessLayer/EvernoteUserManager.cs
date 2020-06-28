using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
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
                    layerResult.Errors.Add("Kullanıcı Adı kayıtlı.");
                }
                if(user.Email == data.Email)
                {
                    layerResult.Errors.Add("E-Posta adresi kayıtlı.");
                }
            }
            else
            {
               int dbResult =  repo_user.Insert(new EvernoteUser()
                {
                    UserName = data.Username,
                    Email = data.Email,
                    Password = data.Password
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
    }
}
