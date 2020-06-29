using MyEvernote.DataAccessLayer;
using MyEvernote.DataAccessLayer.Abstract;
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
    public class Repository<T> : RepositoryBase,IRepository<T> where T : class // T tipi class olmak zorunda
    {
        //private DatabaseContext db = new DatabaseContext();
        private DbSet<T> _objectSet;
        public Repository()
        {
            _objectSet = context.Set<T>();
        }
        public List<T> List()
        {
            return _objectSet.ToList();
        }public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }
        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }
        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            if(obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                DateTime now = DateTime.Now;
                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUserName = "system"; // TODO : İşlem yapan kullanıcı adu yazılmalı..
            }
            return Save();
        }
        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = DateTime.Now;
                o.ModifiedUserName = "system"; // TODO : İşlem yapan kullanıcı adu yazılmalı..
            }
            return Save();
        }
        public int Delete(T obj)
        {
          /*  if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = DateTime.Now;
                o.ModifiedUserName = "system"; // TODO : İşlem yapan kullanıcı adu yazılmalı..
            }*/
            _objectSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
            return context.SaveChanges();
        }
        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

    }
}
