using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Core.DataAccess
{
    // EntityFramework , Sql yada lazım olduğunda başka bir şekle çalışmak için oluşturulmuş İNTERFACE
    public interface IDataAccess<T>
    {
         List<T> List(); // listeleme
        IQueryable<T> ListQueryable(); // listeleme
         List<T> List(Expression<Func<T, bool>> where); // Listeleme
         int Insert(T obj); // ekleme
         int Update(T obj); // güncelleme
         int Delete(T obj); // silme
         int Save(); // kaydetme 
         T Find(Expression<Func<T, bool>> where); // bulma
    }
}
