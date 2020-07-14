using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase,IDataAccess<T> where T : class // T tipi class olmak zorunda
    {
        //private DatabaseContext db = new DatabaseContext();
        private DbSet<T> _objectSet;
        public Repository()
        {
            // CTOR oluşunca veritabanına set işlemi açılsın.
            _objectSet = context.Set<T>(); 
        }
        public List<T> List() // Listeleme işlemi 
        {
            return _objectSet.ToList();
        }public IQueryable<T> ListQueryable() // Listeleme işlemi  
        {
            return _objectSet.AsQueryable<T>();
        }
        public List<T> List(Expression<Func<T,bool>> where) // Listeleme işlemi 
        {
            return _objectSet.Where(where).ToList();
        }
        public int Insert(T obj) // Ekleme işlemi 
        {
            _objectSet.Add(obj);
            if(obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                DateTime now = DateTime.Now;
                o.CreatedOn = now;
                o.ModifiedOn = now;
                //o.ModifiedUserName = "system"; // TODO : İşlem yapan kullanıcı adı yazılmalı..
                o.ModifiedUserName = App.Common.GetCurrentUsername();
            }
            return Save();
        }
        public int Update(T obj) // Güncelleme işlemi 
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = DateTime.Now;
              //  o.ModifiedUserName = "system"; // TODO : İşlem yapan kullanıcı adu yazılmalı..
                o.ModifiedUserName = App.Common.GetCurrentUsername();
            }
            return Save();
        }
        public int Delete(T obj) // Silme işlemi 
        {
            /*  if (obj is MyEntityBase)
              {
                  MyEntityBase o = obj as MyEntityBase;

                  o.ModifiedOn = DateTime.Now;
                  o.ModifiedUserName = "system"; // TODO : İşlem yapan kullanıcı adu yazılmalı..
                   o.ModifiedUserName = App.Common.getUsername();
              }*/
            _objectSet.Remove(obj);
            return Save();
        }
        public int Save() // Kaydetme işlemi
        {
            return context.SaveChanges();
        }
        public T Find(Expression<Func<T, bool>> where) // Bulma işlemi
        {
            return _objectSet.FirstOrDefault(where);
        }

    }
}
