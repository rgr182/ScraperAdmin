using Microsoft.EntityFrameworkCore;
using ScraperAdmin.DataAccess.Context;
using ScraperAdmin.DataAccess.Services;  // Importa los servicios y repositorios
using Microsoft.OpenApi.Models;
using DDEyC_Assistant.Services;
using DDEyC_Assistant.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure the connection to SQL Server using Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the UserService and UserRepository for dependency injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAssistantService, AssistantService>();
// Add services to the container
builder.Services.AddControllersWithViews();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScraperAdmin API", Version = "v1" });
});

// Add CORS policy that accepts any request
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Security Headers Middleware
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'none';");
    await next();
});

app.UseRouting();

// Enable CORS
app.UseCors("AllowAll");

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScraperAdmin API V1");
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
