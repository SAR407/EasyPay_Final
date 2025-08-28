
using EasyPay_Final.Context;
using EasyPay_Final.Interfaces;
using EasyPay_Final.Repositories;
using EasyPay_Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------
// 1️⃣ Database Configuration
// ---------------------------------------------------
builder.Services.AddDbContext<EasypayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------------------------------
// 2️⃣ Register Repositories
// ---------------------------------------------------
// Register Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositoryDB>();
builder.Services.AddScoped<ITimesheetRepository, TimesheetRepositoryDB>();
builder.Services.AddScoped<IPayrollRepository, PayrollRepositoryDB>(); // or PayrollRepositoryDB if that's the actual name
builder.Services.AddScoped<IUserRepository, UserRepositoryDB>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepositoryDB>();
builder.Services.AddScoped<IBenefitRepository, BenefitRepositoryDB>();
builder.Services.AddScoped<IRoleRepository, RoleRepositoryDB>(); // if you have role repo

// ---------------------------------------------------
// 3️⃣ Register Services
// ---------------------------------------------------
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ITimesheetService, TimesheetService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IBenefitService, BenefitService>();
builder.Services.AddScoped<IAuthenticate, AuthenticateService>();

// ---------------------------------------------------
// 4️⃣ AutoMapper
// ---------------------------------------------------
builder.Services.AddAutoMapper(typeof(Program));

// ---------------------------------------------------
// 5️⃣ Authentication - JWT
// ---------------------------------------------------
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// ---------------------------------------------------
// 6️⃣ Authorization - Role Based
// ---------------------------------------------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("HR", policy => policy.RequireRole("HR"));
    options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
});

// ---------------------------------------------------
// 7️⃣ Controllers & Swagger
// ---------------------------------------------------
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EasyPay API", Version = "v1" });

    // JWT Support in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT token with Bearer prefix",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
// ---------------------------------------------------
// 7️⃣ CORS
// ---------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:3000") // React dev server
            .AllowAnyHeader()
            .AllowAnyMethod());
});


var app = builder.Build();

// ---------------------------------------------------
// 8️⃣ Middleware Pipeline
// ---------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
