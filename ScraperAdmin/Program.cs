using Microsoft.EntityFrameworkCore;
using ScraperAdmin.DataAccess.Context;
using ScraperAdmin.DataAccess.Services;
using Microsoft.OpenApi.Models;
using ScraperAdmin.DataAccess.Repositories;

using Microsoft.Extensions.FileProviders;
using ScrapperCron.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<RawHtmlRepositoryOptions>(options =>
{
    options.CollectionName = builder.Configuration["MongoDB:Collections:RawHtmlCollectionName"]?? RawHtmlRepositoryOptions.RawHtml;
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAssistantService, AssistantService>();
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IParserService, ParserService>();
builder.Services.AddScoped<IAIEventProcessingService, AIEventProcessingService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IRawHtmlRepository, RawHtmlRepository>();
builder.Services.AddScoped<IRawHtmlService, RawHtmlService>();
builder.Services.AddScoped<IScraperService, ScraperService>();
builder.Services.AddScoped<IScraperRepository, ScraperRepository>();
builder.Services.AddScoped<ICronService, CronService>();

builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScraperAdmin API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Media")),
    RequestPath = "/Media"
});

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'none';");
    await next();
});

app.UseRouting();

app.UseCors("AllowAll");

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
