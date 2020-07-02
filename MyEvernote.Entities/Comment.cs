using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("Comments")]
    public class Comment : MyEntityBase // MyentityBase'den Id,DateTime CreatedOn,DateTime ModifiedOn, ModifiedUserName geliyor
    {
        [Required,StringLength(300)]
        public string Text { get; set; } // Yorumun içeriği
        public virtual Note Note { get; set; } // yorumun notu
        public virtual EvernoteUser Owner { get; set; } // yorumun sahibi
    }
}
