using AutoMapper;
using BusinessLogic.DTOs;
using DataAccessObject.Models;

namespace FUNewsManagementSystem2.Mapping;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<AdminAccount, AdminAccountDto>();

        CreateMap<SystemAccount, SystemAccountDto>();

        CreateMap<Category, CategoryDto>();

        CreateMap<NewsArticle, NewsArticleDto>()
            .ForMember(dest => dest.SelectedTags, opt => opt.MapFrom(src => src.Tags.Select(t => t.TagId).ToList()));

        CreateMap<NewsArticleDto, NewsArticle>()
            .ForMember(dest => dest.Tags, opt => opt.Ignore());
    }
}