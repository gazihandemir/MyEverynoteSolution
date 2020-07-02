using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
  public class NoteManager
    {
        // Data AccessLayerdaki Repository<T(Generic class)> nesnemizi(Note) oluşturuyoruz.
        private Repository<Note> repo_note = new Repository<Note>();
        // Notları liste şeklinde geri dönmek için oluşturulmuş birbiri ile aynı işlevede 2 fonksiyon.
        public List<Note> getAllNote()
        {
            return repo_note.List();
        }public IQueryable<Note> getAllNoteQueryable()
        {
            return repo_note.ListQueryable();
        }
    }
}
