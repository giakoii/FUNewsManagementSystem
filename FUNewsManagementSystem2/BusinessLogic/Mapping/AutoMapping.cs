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

        CreateMap<Category, CategoryDto>().ReverseMap();
        
        CreateMap<CategoryDto, Category>();
        
        CreateMap<Tag, TagDto>();

        CreateMap<NewsArticle, NewsArticleDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.Tags.Select(t => t.TagId).ToList()))
            .ForMember(dest => dest.TagNames, opt => opt.MapFrom(src => src.Tags.Select(t => t.TagName).ToList()));

        // Mapping từ DTO → Entity (DB)
        CreateMap<NewsArticleDto, NewsArticle>()
            .ForMember(dest => dest.Tags, opt => opt.Ignore());


        CreateMap<NewsArticleDto, NewsArticle>()
            .ForMember(dest => dest.Tags, opt => opt.Ignore());

        CreateMap<NewsArticleDto, NewsArticle>();

        CreateMap<VwUserNewsHistoryDto, ViewUserNewsHistory>();
    }
}