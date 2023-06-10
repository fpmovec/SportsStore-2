using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseSqlServer(ConfigurationExtensions.GetConnectionString(builder.Configuration, "SportsStoreConnection"));
});
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute("catpage",
        "{category}/Page{productPage:int}", new {Controller = "Home", action = "Index"});
	endpoints.MapControllerRoute("page",
		"Page{productPage:int}", new { Controller = "Home", action = "Index", productPage = 1});
	endpoints.MapControllerRoute("category",
	"{category}", new { Controller = "Home", action = "Index", productPage = 1 });
	endpoints.MapControllerRoute("pagination",
        "Products/Page{productPage}", new { Controller = "Home", action = "Index", productPage = 1});
    endpoints.MapDefaultControllerRoute(); 
});
SeedData.EnsurePopulated(app);
app.Run();
