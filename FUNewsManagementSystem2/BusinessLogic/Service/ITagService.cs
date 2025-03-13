using BusinessLogic.DTOs;
using BusinessObject.Service;
using DataAccessObject.Models;

namespace BusinessLogic.Service;

public interface ITagService : IBaseService<Tag, int>
{
    List<TagDto> GetTags();
}