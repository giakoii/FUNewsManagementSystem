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
        private readonly TagsRepository _tagRepository;

        public TagService(TagsRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

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
