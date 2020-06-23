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
            //Adding admin user
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
            //Adding standart user
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
            for(int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user{i}",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUserName = $"user{i}"
                };
                context.EvernoteUsers.Add(user);
            }
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
                context.Categories.Add(cat);
                // adding fake notes
                for (int k = 0;k< FakeData.NumberData.GetNumber(5, 10); k++)
                {
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Category = cat,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = (k % 2 == 0) ? admin : standartUser,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                        ModifiedUserName = (k % 2 == 0) ? admin.UserName : standartUser.UserName
                    };
                    cat.Notes.Add(note);
                    // Adding fake comments
                    for(int j = 0;j<FakeData.NumberData.GetNumber(3,5); j++)
                    {
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Note = note,
                            Owner = (j % 2 == 0) ? admin : standartUser,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUserName = (j % 2 == 0) ? admin.UserName : standartUser.UserName
                        };
                        note.Comments.Add(comment);
                    }
                    // Adding fake Likes
                    List<EvernoteUser> userlist = context.EvernoteUsers.ToList();
                    for (int m = 0; m < note.LikeCount ; m++) 
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]

                        };
                        note.Likes.Add(liked);
                    }
                }
            }
            context.SaveChanges();
        }
        public override void InitializeDatabase(DatabaseContext context)
        {
            base.InitializeDatabase(context);
        }
       
    }
}
