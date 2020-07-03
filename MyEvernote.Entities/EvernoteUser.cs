using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntityBase // MyentityBase'den Id,DateTime CreatedOn,DateTime ModifiedOn, ModifiedUserName geliyor
    {
        [StringLength(25)]
        public string  Name { get; set; } // Kullanıcının ismi
        [StringLength(25)]
        public string  Surname { get; set; }  // Kullanıcının soyismi
        [Required,StringLength(25)]
        public string  UserName { get; set; }  // Kullanıcının kullanıcı adı
        [Required,StringLength(70)]
        public string  Email { get; set; }  // Kullanıcının E-postası
        [StringLength(25),Required]
        public string  Password { get; set; }  // Kullanıcının şifresi
        [StringLength(30)] // /images/user_12.jpg
        public string ProfileImageFilename { get; set; } // kullanıcının fotoğrafı
        public bool IsActive { get; set; }  // Kullanıcını aktivasyon kodunu onaylamış mı ? 
        public bool IsAdmin { get; set; } // Kullanıcı Admin mi ? 
        [Required]
        public Guid ActivateGuid { get; set; } // Kullanıcının aktivasyon kodu

        public virtual List<Note> Notes { get; set; } // Kullanıcının notu
        public virtual List<Comment> Comments { get; set; }  // Kullanıcının yorumu
        public virtual List<Liked> Likes { get; set; }  // Kullanıcının beğendikleri
    } 
}
