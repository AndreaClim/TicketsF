using Microsoft.EntityFrameworkCore;
using TicketsF.Models;
using TicketsF.Services;
using TicketsF.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Variable de sesi�n
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(3600);
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});

// Configura los servicios y la base de datos
builder.Services.AddDbContext<ticketsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TicketsDbConnection")));

builder.Services.AddScoped<correo>();

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

app.UseAuthorization();

app.UseSession(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
