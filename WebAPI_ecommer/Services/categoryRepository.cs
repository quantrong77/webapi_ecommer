
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_ecommer.Data;
using WebAPI_ecommer.Models;

namespace WebAPI_ecommer.Services
{
    public class categoryRepository : IcategoryRepository
    {
        private readonly myDBContext _context;

        public categoryRepository(myDBContext context)
        {

            _context = context;
        }
        public CategoryModel Add(Category product_Category)
        {
            var _category = new Category
            {
                CategoryName = product_Category.CategoryName
            };
            _context.Add(_category);
            _context.SaveChanges();
            return new CategoryModel
            {
                Id = _category.Id,
                CategoryName = _category.CategoryName
            };
        }

        public void Delete(int id)
        {
            var category = _context.ProductCategories.SingleOrDefault(lo => lo.Id == id);
            if (category != null)
            {
                _context.Remove(category);
                _context.SaveChanges();
            }
                
        }

        public List<CategoryModel> GetAll()
        {
           var categorys = _context.ProductCategories.Select(ca => new CategoryModel { 
                Id = ca.Id,
                CategoryName = ca.CategoryName

            }) ;
            return categorys.ToList();
        }

        public CategoryModel GetById(int id)
        {
            var category = _context.ProductCategories.SingleOrDefault(lo => lo.Id == id);
            if (category != null)
            {
                return new CategoryModel
                {
                    Id = category.Id,
                    CategoryName= category.CategoryName
                };
            }
            return null;

        }

        public void Update(CategoryModel product_Category)
        {
        
        }
    }
}
