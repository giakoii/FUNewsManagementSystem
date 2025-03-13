using AutoMapper;
using BusinessLogic.DTOs;
using DataAccessObject.Models;
using FUNewsManagementSystem.Models.ViewModel;
using FUNewsManagementSystem2.ViewModel;

namespace FUNewsManagementSystem2.Mapping;

public class ViewModelMapping : Profile
{
    public ViewModelMapping()
    {
        CreateMap<SystemAccountDto, SystemAccountViewModel>();
        
        CreateMap<SystemAccountViewModel, SystemAccountDto>();

        CreateMap<CategoryViewModel, CategoryDto>();

        CreateMap<CategoryDto, CategoryViewModel>();

        CreateMap<TagDto, TagViewModel>();
        
        CreateMap<NewsArticleDto, NewsArticleViewModel>();

        // Mapping từ ViewModel (UI) → DTO
        CreateMap<NewsArticleViewModel, NewsArticleDto>();
        
        CreateMap<NewsArticle, NewsArticleViewModel>();
        
        CreateMap<NewsArticleViewModel, NewsArticle>();

        CreateMap<TagViewModel, Tag>();
    }
}