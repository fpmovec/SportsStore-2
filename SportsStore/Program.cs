using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseSqlServer(ConfigurationExtensions.GetConnectionString(builder.Configuration, "SportsStoreConnection"));
});
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddServerSideBlazor();

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseRouting();
app.UseSession();
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
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");
});
SeedData.EnsurePopulated(app);
app.Run();
