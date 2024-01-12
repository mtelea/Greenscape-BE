using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Project1.Data;
using Project1.Mapper;
using Project1.Model;
using Project1.Service;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var secretConfiguration = new ConfigurationBuilder()
    .AddJsonFile("secrets.json")
    .Build();

builder.Services.AddScoped<PlantMapper>();
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(secretConfiguration["DefaultConnection"]));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>
    (options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = true;

    })
/*.AddRoles<IdentityRole>()*/
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization();
builder.Services.AddScoped<PointsHistory>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7211",
                                              "https://localhost:44488")
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IEmailSender>(provider =>
{
    /*var configuration = provider.GetRequiredService<IConfiguration>();
    var apiKey = configuration["SendGrid:ApiKey"];*/
    return new SendGridEmailSender(secretConfiguration["SendGrid_APIKey"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API");
});

/*app.MapIdentityApi<ApplicationUser>();*/
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;
app.UseStaticFiles();
app.UseAuthorization();


app.Run();
