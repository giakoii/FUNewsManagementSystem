using BusinessObject.Service;
using DataAccessObject.Models;
using DataAccessObject.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FUNewsManagementSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
         options.LoginPath = "/Home/Index";
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
