using LogApi.Entities;
using LogApi.Models;
using LogApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.iis.json", true);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppLogsContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbCnn")));

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = builder.Configuration.GetSection("AutomapperKey").Value, typeof(Program));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
builder.Services.AddScoped<ILogDataService, LogDataService>();

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
