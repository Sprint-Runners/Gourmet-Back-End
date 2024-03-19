using Microsoft.EntityFrameworkCore;
using Gourmet.Core.Services;
using Gourmet.Core.ServiceContracts;
using Gourmet.Infrastructure.GourmetDbcontext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173/");
    });
});
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default-Ali"));
});
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddSingleton<IJwt,JWTService>();



builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
