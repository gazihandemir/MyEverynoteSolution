using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyEvernote.Entities
{
    [Table("Notes")]
   public class Note : MyEntityBase  // MyentityBase'den Id,DateTime CreatedOn,DateTime ModifiedOn, ModifiedUserName geliyor
    {
        [DisplayName("Not Başlığı") ,Required,StringLength(60)]
        public string Title { get; set; } // notun başlığı
        [DisplayName("Not Metni"), Required,StringLength(2000)]
        public string Text { get; set; } // notun içeriği
        [DisplayName("Taslak")]
        public bool IsDraft { get; set; } // not taslak mı ? 
        [DisplayName("Beğenilme")]
        public int  LikeCount{ get; set; } // Begeni sayısı
        [DisplayName("Kategori")]
        public int CategoryId { get; set; } // Hangi kategoriye bağlı olduğunu anlamak için id
        public virtual EvernoteUser Owner { get; set; } // Hangi kullanıcıya(owner(sahibi)) ait
        public virtual List<Comment> Comments { get; set; } // Notun yorumları(list)
        public virtual Category Category { get; set; } // Notun kategorisi
        public virtual List<Liked> Likes{ get; set; } // Notun beğenileri
        public Note()
            // CTOR oluştuğunda Yorum ve beğenme listeleri oluşsun
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }
    }
}
