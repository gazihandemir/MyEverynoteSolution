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
    [Table("Categories")]
    public class Category : MyEntityBase  // MyentityBase'den Id,DateTime CreatedOn,DateTime ModifiedOn, ModifiedUserName geliyor
    {
        [DisplayName("Kategori"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter içermelidir.")]
        public string Title { get; set; } // kategorinin başlığı
        [DisplayName("Açıklama"),
            StringLength(150, ErrorMessage = "{0} alanı max. {1} karakter içermelidir.")]
        public string Description { get; set; }  // kategorinin açıklaması
        public virtual List<Note> Notes { get; set; }  // kategorinin başlığı
        public Category()
        {
            // CTOR kategori oluştuğunda Note türünden bir list oluşsun.
            Notes = new List<Note>();
        }
    }
}
