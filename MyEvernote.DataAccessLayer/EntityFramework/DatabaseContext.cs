﻿using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    // Veri tabanımıza kaydedeceğimiz tüm verilerimizin başlıkları
    {
        public DbSet<EvernoteUser> EvernoteUsers { get; set; } // Kullanıcılar
        public DbSet<Note> Notes { get; set; } // Kullanıcıların yazdığı notlar
        public DbSet<Comment> Comments { get; set; } //  Kullanıcıların yazdığı yorumlar
        public DbSet<Category> Categories { get; set; } // Kategoriler
        public DbSet<Liked> Likes { get; set; } // Kullanıcıların yazdığı notlar için beğenmeler
        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }
 /*       protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // FluentAPI 
            modelBuilder.Entity<Note>()
              .HasMany(n => n.Comments) // çok ilişkilidir comments tablosuyla
              .WithRequired(c => c.Note) // commentin notu olmak zorundadır
              .WillCascadeOnDelete(true); // not silinince yorumlarıda sil bağlantısı
            modelBuilder.Entity<Note>()
                .HasMany(n => n.Likes)
                .WithRequired(c => c.Note)
                .WillCascadeOnDelete(true);
        }
 */
    }

}
