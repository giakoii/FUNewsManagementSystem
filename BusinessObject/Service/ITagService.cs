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
        IEnumerable<Tag> GetAllTags();
        Tag GetTagById(int id);
        bool AddTag(Tag tag);
        void UpdateTag(Tag tag);
        bool DeleteTag(int id);
        IEnumerable<Tag> GetTagsByIds(List<int> ids);
    }
}
