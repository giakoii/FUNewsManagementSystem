using DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAllTag();
        Tag GetTagById(int id);
        bool AddNewsArticle(Tag tag);
        void UpdateNewsArticle(Tag tag);
        bool DeleteNewsArticle(int id);
    }
}
