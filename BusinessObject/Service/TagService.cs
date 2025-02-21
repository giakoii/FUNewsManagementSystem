using DataAccessObject.Models;
using DataAccessObject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagRepository;

        public TagService(IRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _tagRepository.GetAll() ?? Enumerable.Empty<Tag>();
        }

        public Tag GetTagById(int id) => _tagRepository.GetById(id);

        public bool AddTag(Tag tag) => _tagRepository.Add(tag);

        public void UpdateTag(Tag tag) => _tagRepository.Update(tag);

        public bool DeleteTag(int id) => _tagRepository.Delete(id);

        public IEnumerable<Tag> GetTagsByIds(List<int> ids)
            => _tagRepository.GetAll(x => ids.Contains(x.TagId)).ToList();
    }
}
