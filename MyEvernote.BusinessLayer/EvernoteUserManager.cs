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
        public EvernoteUser RegisterUser(RegisterViewModel data)
        {
            // Kullanıcı username kontrolü
            // Kullanıcı eposta kontrolü
            // Kayıt işlemi
            // Aktivasyon e postası gönderimi
            EvernoteUser user =  repo_user.Find(x => x.UserName == data.Username || x.Email == data.Email);
            if(user != null)
            {
                throw new Exception("Kayıtlı kullanıcı adı ya da e-posta adresi");
            }
        }
    }
}
