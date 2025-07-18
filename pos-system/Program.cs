using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.Helpers;
using pos_system.Mapping;
using pos_system.Models;
using pos_system.Repository;
using pos_system.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/NotFoundPage";
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
});

builder.Services.Configure<MidtransSettings>(
    builder.Configuration.GetSection("Midtrans")
);

builder.Services.AddScoped<IProductCategoryRepo, ProductCategoryRepo>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped(typeof(ICrudRepo<>), typeof(CrudRepo<>));
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IVariantGroupRepo, VariantGroupRepo>();
builder.Services.AddScoped<IOrderNumberTrackerRepo, OrderNumberTrackerRepo>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IProductVariantRepo, ProductVariantRepo>();
builder.Services.AddScoped<IInventoryRepo, InventoryRepo>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ILogAuditRepo, LogAuditRepo>();
builder.Services.AddScoped<ILogAuditService, LogAuditService>();
builder.Services.AddScoped<ISetupRepo, SetupRepo>();
builder.Services.AddScoped<ISetupService, SetupService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

using (var scope = app.Services.CreateScope())
{
    await Unique.SeedRoles(scope.ServiceProvider);
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.MapHub<OrderHub>("/orderHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
