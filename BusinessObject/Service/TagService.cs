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

        private readonly TagsRepository _tagRepository;

        public TagService(TagsRepository tagRepository)

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

        public bool AddNewsArticle(Tag tag)
        {
            throw new NotImplementedException();
        }

        public bool DeleteNewsArticle(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tag> GetAllTag()
        {
            return _tagRepository.GetAll();
        }

        public Tag GetTagById(int id)
        {
            return _tagRepository.GetById(id);
        }

        public void UpdateNewsArticle(Tag tag)
        {
            throw new NotImplementedException();
        }

    }
}
