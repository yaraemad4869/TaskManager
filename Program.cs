using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TaskManager.Controllers;
using System.Text.Json.Serialization;
using TaskManager.Core.Models;
using TaskManager.Core.Interfaces;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Data;
using TaskManager.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Auth.Abstracts;
using TaskManager.Auth.Implementation;
using TaskManager.Extensions;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ConStr") ?? throw new InvalidOperationException("Connection string 'ConStr' not found.");;

builder.Services.AddDbContext<AppDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorizationPolicies();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
// Add services to the container.
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IBasicRepo<User>, BasicRepo<User>> ();
builder.Services.AddTransient<IBasicRepo<Category>, BasicRepo<Category>>();
builder.Services.AddTransient<IBasicRepo<Color>, BasicRepo<Color>>();
builder.Services.AddTransient<IBasicRepo<Item>, BasicRepo<Item>>();
builder.Services.AddTransient<IBasicRepo<ToDoList>, BasicRepo<ToDoList>>();
builder.Services.AddTransient<IBasicRepo<ToDo>, BasicRepo<ToDo>>();
builder.Services.AddTransient<IBasicRepo<Note>, BasicRepo<Note>>();
builder.Services.AddTransient<IItemRepo<Note>, ItemRepo<Note>>();
builder.Services.AddTransient<IItemRepo<ToDoList>, ItemRepo<ToDoList>>();
builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddTransient<ICategoryRepo, CategoryRepo>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}

//app.UsePathBase("/swagger");
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run();
