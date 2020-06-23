using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEvernote.Entities;

namespace MyEvernote.DataAccessLayer
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Gazihan",
                Surname = "Demir",
                Email = "gazihand@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName = "gazihandemir",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "gazihandemir"

            };
            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Cansu",
                Surname = "Demir",
                Email = "cansu@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                UserName = "cansudemir",
                Password = "654321",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUserName = "cansudemir"
            };
            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);
            context.SaveChanges();
            // adding fake categories
            for(int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUserName = "gazihandemir"
                };

                for(int k = 0;k< FakeData.NumberData.GetNumber(5, 10); k++)
                {
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Category = cat,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(10, 50),
                        Owner = (k % 2 == 0) ? admin : standartUser,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                        ModifiedUserName = (k % 2 == 0) ? admin.UserName : standartUser.UserName
                    };
                    cat.Notes.Add(note);
                }
            }
        }
        public override void InitializeDatabase(DatabaseContext context)
        {
            base.InitializeDatabase(context);
        }
       
    }
}
