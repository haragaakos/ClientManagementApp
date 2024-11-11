global using Microsoft.EntityFrameworkCore;
global using ClientManagementApp.Data;
global using ClientManagementApp.Models;
global using Microsoft.AspNetCore.Mvc;
global using ClientManagementApp.Services;
global using Newtonsoft.Json;
global using System.ComponentModel.DataAnnotations.Schema;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<JsonGenerator>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Client}/{action=Index}/{id?}");

app.Run();
