using System;
using System.Collections.Generic;
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
        [Required,StringLength(50)]
        public string  Title{ get; set; } // kategorinin başlığı
        [StringLength(150)]
        public string Description{ get; set; }  // kategorinin açıklaması
        public virtual List<Note> Notes{ get; set; }  // kategorinin başlığı
        public Category()
        {
            // CTOR kategori oluştuğunda Note türünden bir list oluşsun.
            Notes = new List<Note>();
        }
    }
}
