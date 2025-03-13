using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Service;
using DataAccessObject.Models;
using DataAccessObject.Repositories;

namespace BusinessObject.Service
{
    public class TagService : BaseService<Tag, int>, ITagService
    {
        private readonly IMapper _mapper;
        public TagService(BaseRepository<Tag, int> repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        public List<TagDto> GetTags()
        {
            var tags = GetBy().ToList();
            return _mapper.Map<List<TagDto>>(tags);
        }
    }
}
