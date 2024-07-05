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
using Microsoft.OpenApi.Models;
using RapidApiExample.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

var proxy = new WebProxy("http://127.0.0.1:9050")
{
    UseDefaultCredentials = false
};


var httpClientHandler = new HttpClientHandler
{
    Proxy = proxy,
    UseProxy = true
};


builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
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

void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer("Default-Ali");
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default-Ali"));
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
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidateLifetime = false,
               ValidateIssuerSigningKey = true,
               ValidIssuer = "http://localhost:5286",
               //ValidAudience = "Front",
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GourmetProjectKeykfnklflerhguirhguihguhr"))
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
builder.Services.AddHttpClient();
//sk-proj-c3TVsXBxKivphLctG1hXT3BlbkFJ5bWlfJQ9FJSqvJ6paBjf
//sk-gormetwebsite-BMZFeovFtffBCpIvZisCT3BlbkFJwO7VZGuism14fTqQwzQ9
//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


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
//builder.Services.AddHttpClient<RapidApiService>();
builder.Services.AddHttpClient("RapidApiClient")
                .ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);


builder.Services.AddTransient<RapidApiService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("RapidApiClient");
    return new RapidApiService(httpClient);
});


var app = builder.Build();



    //    app.UseSwagger();
    //    app.UseSwaggerUI(c => {
    //	    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    //	    c.RoutePrefix = "docs";
    //}
    //	);

        app.UseSwagger();
        app.UseSwaggerUI();

    



// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.UseStaticFiles();

app.MapControllers();

app.Run();
