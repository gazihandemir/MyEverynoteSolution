using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("Likes")]
    public class Liked
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Anahtar , veritabanı oluşurken anahtar şeklinde oluşturulsun
        // Yani otomatik artan 
        public int Id { get; set; } // Begeninin ıd'si
        public Note Note { get; set; } // Begeninin notu
        public EvernoteUser LikedUser { get; set; } // Begeninin sahibi
    }
}
