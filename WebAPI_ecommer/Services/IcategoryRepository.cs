using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_ecommer.Models;

namespace WebAPI_ecommer.Services
{
    public interface IcategoryRepository
    {
        List<CategoryModel> GetAll();
        CategoryModel GetById(int id);
        CategoryModel Add(Category product_Category);
        void Update(CategoryModel product_Category);
        void Delete(int id);
    }
}
