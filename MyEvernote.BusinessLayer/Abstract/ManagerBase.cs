using MyEvernote.Core.DataAccess;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer.Abstract
{
    public abstract class ManagerBase<T> : IDataAccess<T> where T : class
    {
        // Controllerdaki açacağımız managerleri her seferinde ayrı ayrı delete,find gibi
        // veri tabanı kodlarını yazmak yerine ManagerBaseye yazıyoruz ve diğer Managerlerden
        // referans alıyoruz . Fonksiyonlardan faydalanıyoruz.

        private Repository<T> repo = new Repository<T>(); 
        public virtual int Delete(T obj) // Silme
        {
            return repo.Delete(obj);
        }

        public virtual T Find(Expression<Func<T, bool>> where) // Bulma
        {
            return repo.Find(where);
        }

        public virtual int Insert(T obj) // Ekleme
        {
            return repo.Insert(obj);
        }

        public virtual List<T> List() // Listeleme
        {
            return repo.List();
        }

        public virtual List<T> List(Expression<Func<T, bool>> where) // Listeleme
        {
            return repo.List(where);
        }

        public virtual IQueryable<T> ListQueryable() // Listeleme
        {
            return repo.ListQueryable();
        }

        public virtual int Save() // kaydetme
        {
            return repo.Save();
        }

        public virtual int Update(T obj) // Güncelleme
        {
            return repo.Update(obj);
        }
    }
}
