using MyEvernote.DataAccessLayer.EntityFramework;
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
        Repository<Comment> repo_comment = new Repository<Comment>();
        Repository<Note> repo_note = new Repository<Note>();
        public test()
        {
            //   DataAccessLayer.DatabaseContext db = new DataAccessLayer.DatabaseContext();
            //   db.Categories.ToList();

            //repo.List(x => x.Id > 5);
            List<Category> categories = repo_category.List();
            //  List<Category> categories_filtered = repo_category.List(x => x.Id > 5);

        }
        public void InsertTest()
        {

            int result = repo_user.Insert(new EvernoteUser()
            {
                Name = "aaaa",
                Surname = "bbbb",
                Email = "aabb@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName = "aabb",
                Password = "111",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "aabb"
            });
        }
        public void UpdateTest()
        {
            EvernoteUser user = repo_user.Find(x => x.UserName == "aabb");
            if (user != null)
            {
                user.UserName = "xxx";
                int result = repo_user.Update(user);
            }
        }
        public void DeleteTest()
        {
            EvernoteUser user = repo_user.Find(x => x.UserName == "xxx");
            if (user != null)
            {
                int result = repo_user.Delete(user);
            }
        }
        public void commentTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Id ==  1 );
            Note note = repo_note.Find(x => x.Id == 3);
            Comment comment = new Comment()
            {
                Text = "Bu bir test",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUserName = "gazihan",
                Note = note,
                Owner = user
            };
            repo_comment.Insert(comment);
        }
    }
}
