using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
  public class MyEntityBase
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Anahtar , veritabanı oluşurken anahtar şeklinde oluşturulsun
        // Yani otomatik artan 
        public int Id { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; } // Oluşturulma zamanı
        [Required]
        public DateTime ModifiedOn { get; set; } // Değiştirilme zamanı
        [Required,StringLength(30)]
        public string ModifiedUserName { get; set; } // Değiştirenin kullanıcı adı 
    }
}
