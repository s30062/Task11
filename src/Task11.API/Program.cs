using System.Text;
using EmployeeManager.API;
using EmployeeManager.Repository.context;
using EmployeeManager.Repository.interfaces;
using EmployeeManager.Repository.repositories;
using EmployeeManager.Services.dtos;
using EmployeeManager.Services.interfaces;
using EmployeeManager.Services.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// cononfiguration: JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

//  DbContext
var connectionString = builder.Configuration.GetConnectionString("EmployeeDatabase");
builder.Services.AddDbContext<EmployeeDatabaseContext>(options => options.UseSqlServer(connectionString));

// ddependency Injection
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

// swagger + MVC
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//  JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var jwtSection = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["SecretKey"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); 


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeDatabaseContext>();
    await RoleSeeder.SeedRolesAsync(dbContext);
}

app.Run();
