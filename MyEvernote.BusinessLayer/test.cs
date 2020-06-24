using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class test
    {
       private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        Repository<Category> repo_category = new Repository<Category>();
        public test()
        {
            //   DataAccessLayer.DatabaseContext db = new DataAccessLayer.DatabaseContext();
            //   db.Categories.ToList();
            
            //repo.List(x => x.Id > 5);
            List<Category> categories= repo_category.List();

        }
        public void InsertTest()
        {
            
            int result = repo_user.Insert(new EvernoteUser()
            {
                Name="aaaa",
                Surname="bbbb",
                Email="aabb@gmail.com",
                ActivateGuid=Guid.NewGuid(),
                IsActive=true,
                IsAdmin=true,
                UserName="aabb",
                Password="111",
                CreatedOn=DateTime.Now,
                ModifiedOn= DateTime.Now.AddMinutes(5),
                ModifiedUserName = "aabb"
            });
        }
        public void UpdateTest()
        {
            EvernoteUser user = repo_user.Find(x => x.UserName == "aabb");
            if(user != null)
            {
                user.UserName = "xxx";
                repo_user.Save();
            }
        }
    }
}
