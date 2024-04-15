using IBuyStuff.Application.Services;
using IBuyStuff.Application.Services.Authentication;
using IBuyStuff.Application.Services.Home;
using IBuyStuff.Application.Services.Order;
using IBuyStuff.Domain.Repositories;
using IBuyStuff.Domain.Services;
using IBuyStuff.Domain.Services.Impl;
using IBuyStuff.Infrastructure;
using IBuyStuff.Infrastructure.Hashing;
using IBuyStuff.Persistence.Facade;
using IBuyStuff.Persistence.Repositories;
using IBuyStuff.Persistence.Utils;
using IBuyStuff.Server.Common.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<DomainModelFacade>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = IdentityConstants.ApplicationScheme;
}).AddTwitter(o => 
{
    var cfg = builder.Configuration.GetRequiredSection("naa4e:twitter");
    o.ConsumerKey = GetRequiredValue(cfg, "key");
    o.ConsumerSecret = GetRequiredValue(cfg, "sec");
}).AddFacebook(o => 
{
    var cfg = builder.Configuration.GetRequiredSection("naa4e:fb");
    o.AppId = GetRequiredValue(cfg, "key");
    o.AppSecret = GetRequiredValue(cfg, "sec");
}).AddIdentityCookies(b => 
{ 
    b.ApplicationCookie!.Configure(o =>
    {
        o.LoginPath = "/login";
    }); 
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//Provide the same functionality as the Microsoft.AspNet.Web.Optimization component,
//which does not have a corresponding version in .NET Core.
builder.Services.AddWebOptimizer(pipeline => 
{ 
    pipeline.AddJavaScriptBundle("~/Bundles/Core",
        "/Content/Scripts/jquery-1.10.2.min.js",
        "/Content/Scripts/ibuystuff.js").UseContentRoot();

    pipeline.AddJavaScriptBundle("~/Bundles/Bootstrap",
    "/Content/Scripts/bootstrap.min.js").UseContentRoot();

    pipeline.AddCssBundle("~/Bundles/Css",
        "/Content/Styles/bootstrap.min.css",
        "/Content/Styles/site.css").UseContentRoot();
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISecurityStampValidator, DefaultSecurityStampValidator>();
builder.Services.AddScoped<IHashingService, DefaultPasswordHasher>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IOrderRequestService, OrderRequestService>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();

builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IHomeControllerService, HomeControllerService>();
builder.Services.AddScoped<ILoginControllerService, LoginControllerService>();
builder.Services.AddScoped<IOrderControllerService, OrderControllerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var sp = app.Services.CreateScope();
    var db = sp.ServiceProvider.GetRequiredService<DomainModelFacade>();
    //db.Database.EnsureDeleted();
    if (db.Database.EnsureCreated())
    {
        SampleAppInitializer.Seed(db, sp.ServiceProvider.GetRequiredService<IHashingService>(), g => IdentityHelpers.GetAvatar(g));
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseWebOptimizer(); // used to replace Microsoft.AspNet.Web.Optimization.
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();

string GetRequiredValue(IConfigurationSection cfg, string name)
{
    var value = cfg.GetRequiredSection(name).Value;
    if (String.IsNullOrWhiteSpace(value))
        throw new InvalidOperationException($"The value of Configuration '{name}' cannot be empty.");

    return value;
}