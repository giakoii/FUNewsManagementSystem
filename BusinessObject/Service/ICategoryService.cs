using DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategory();
        IEnumerable<Category> GetAllSubCategory(int categoryId);
    }
}
