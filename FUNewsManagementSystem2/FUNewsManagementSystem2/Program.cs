using BusinessLogic.Service;
using BusinessObject.Service;
using DataAccessObject.Models;
using DataAccessObject.Repositories;
using FUNewsManagementSystem.Hubs;
using FUNewsManagementSystem2.Mapping;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext
builder.Services.AddDbContext<FUNewsManagementSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Cấu hình AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(AutoMapping), typeof(ViewModelMapping));


// ✅ Sử dụng Razor Pages thay vì MVC
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Đăng ký các Service (Dependency Injection)
builder.Services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
builder.Services.AddScoped<INewArticleService, NewArticleService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<FUNewsManagementSystemContext>();

builder.Services.Configure<AdminAccount>(builder.Configuration.GetSection("AdminAccount"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     .AddCookie(options =>
     {
         options.LoginPath = "/Login";  
         options.ExpireTimeSpan = TimeSpan.FromDays(7);
         options.SlidingExpiration = true;
         options.Cookie.IsEssential = true;
     });

builder.Services.AddScoped(typeof(BaseRepository<,>));
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<INewArticelRepository, NewArticleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagsRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); 
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NewsHub>("/newshub");
    endpoints.MapRazorPages();
    endpoints.MapFallbackToPage("/Auth/Login");
});

app.MapRazorPages();

app.Run();
