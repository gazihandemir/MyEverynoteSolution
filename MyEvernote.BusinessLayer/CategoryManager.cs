using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager
    {
        // Data AccessLayerdaki Repository<T(Generic class)> nesnemizi(Category) oluşturuyoruz.
        private Repository<Category> repo_category = new Repository<Category>();
        // Categoryleri liste şeklinde getiren ve List dönen getCategories() fonksiyon.
        public List<Category> getCategories()
        {
            return repo_category.List(); // List -> Repository fonksiyon.
        }
        // Categorylerin ID'lerini  bulan ve Category döndüren  getCategoryById() fonksiyonu.
        public Category getCategoryById(int id)
        {
            return repo_category.Find(x => x.Id == id); // Find -> Repository fonksiyon.
        }
    }
}
