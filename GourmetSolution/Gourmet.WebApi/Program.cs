using Microsoft.EntityFrameworkCore;
using Gourmet.Core.Services;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Gourmet.Core.DataBase.GourmetDbcontext;
using Google;
using Gourmet.Core.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        });
});

void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer("Default-Hengameh");
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default-Hengameh"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

// Add Identity
builder.Services
    .AddIdentity<Chef, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


// Config Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    //options.SignIn.RequireConfirmedEmail = true;
});
// Add Authentication and JwtBearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = "YourIssuer",
               ValidAudience = "YourAudience",
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey"))
           };
       });
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IJwt, JWTService>();
builder.Services.AddScoped<IChefService, ChefService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IImageProcessorService, ImageProcessorService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();



builder.Services.AddControllers();
//builder.Services.AddCors(options =>
//    {
//        options.AddPolicy("AllowSpecificOrigin",
//            builder =>
//            {
//                builder.WithOrigins("http://localhost:5173")
//                       .AllowAnyHeader()
//                       .AllowAnyMethod();
//            });
//    });
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.UseStaticFiles();

app.MapControllers();

app.Run();