using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("İsim"),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; } // Kullanıcının ismi
        [DisplayName("Soyad"),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Surname { get; set; }  // Kullanıcının soyismi
        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string UserName { get; set; }  // Kullanıcının kullanıcı adı
        [DisplayName("E-posta"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }  // Kullanıcının E-postası
        [DisplayName("Şifre"),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır."),
            Required(ErrorMessage = "{0} alanı gereklidir.")]
        public string Password { get; set; }  // Kullanıcının şifresi
        [StringLength(30),
            ScaffoldColumn(false)] // /images/user_12.jpg
        public string ProfileImageFilename { get; set; } // kullanıcının fotoğrafı
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }  // Kullanıcını aktivasyon kodunu onaylamış mı ? 
        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; } // Kullanıcı Admin mi ? 
        [Required,
            ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; } // Kullanıcının aktivasyon kodu

        public virtual List<Note> Notes { get; set; } // Kullanıcının notu
        public virtual List<Comment> Comments { get; set; }  // Kullanıcının yorumu
        public virtual List<Liked> Likes { get; set; }  // Kullanıcının beğendikleri
    }
}
