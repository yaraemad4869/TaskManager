using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Notes.Data.Models;
using TaskManager.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TaskManager.Data.Models;
using TaskManager.Controllers;
using TaskManager.IRepositories;
using TaskManager.Repositories;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ConStr") ?? throw new InvalidOperationException("Connection string 'ConStr' not found.");;

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
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
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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
app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run();
