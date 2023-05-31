using MentalHealthApp.DataAccess;
using MentalHealthApp.DataAccess.Context;
using MentalHealthApp.WebApp.Code;
using MentalHealthApp.WebApp.Code.ExtensionMethods;
using MentalHealthApp.WebApp.WebRTCHub;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilterAttribute));
});
builder.Services.AddDbContext<MentalHealthAppContext>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddPresentation();
builder.Services.AddMentalHealthAppCurrentUser();
builder.Services.AddMentalHealthAppBusinessLogic();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(Program).Assembly);



builder.Services.AddAuthentication("MentalHealthAppCookies")
        .AddCookie("MentalHealthAppCookies", options =>
        {
            options.AccessDeniedPath = new PathString("/Account/Login");
            options.LoginPath = new PathString("/Account/Login");
            options.LoginPath = new PathString("/Account/AdminLogin");

        });
        
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
   
  
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Privacy}/{id?}");
app.MapHub<WebRTCHub>("/WebRTCHub");
app.Run();
