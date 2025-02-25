using DataAccessObject.Models;
using DataAccessObject.Repositories;

namespace BusinessObject.Service
{
    public class TagService : BaseService<Tag, int>, ITagService
    {
        public TagService(BaseRepository<Tag, int> repository) : base(repository)
        {
        }
    }
}
