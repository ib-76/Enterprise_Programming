using DataAccess.Context;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common.Interfaces;
using System.Reflection.Metadata.Ecma335;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ShoppingCartDbContext>();
builder.Services.AddControllersWithViews();

//scoped services =>  different instance per http request that is most commonly used for db context objects
//Singleton services => one instnce to be shared by all components that require it
// If two users request the same singleton service at the same time, they will share the same instance, which may
// cause one user to wait until the other finishes.



string productsFilePath = builder.Configuration["productJsonPath"].ToString();

string implementationChoice = builder.Configuration["dataSource"].ToString();

var host = builder.Environment;
string absoluteProductsFilePath =
    Path.Combine(host.ContentRootPath, productsFilePath);

builder.Services.AddScoped<CategoriesRepository>();

switch (implementationChoice)
{
    case "db":
        builder.Services.AddScoped<IProductsRepository, ProductsDbRepository>();
        break;

    case "json":
        builder.Services.AddScoped<IProductsRepository,
            ProductsFileRepository>(options => { return new ProductsFileRepository(absoluteProductsFilePath); });
        break;

    case "cache":
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<IProductsRepository, ProductsCacheRepository>();
        break;

}


        builder.Services.AddScoped(typeof(DataAccess.Repositories.OrdersRepository));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");// https://localhost:xxxx/Products/Index
app.MapRazorPages();

app.Run();
