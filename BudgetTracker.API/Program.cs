using BudgetTracker.BL.Services.Interfaces;
using BudgetTracker.BL.Services;
using BudgetTracker.Core;
using Microsoft.EntityFrameworkCore;
using BudgetTracker.DAL.Services.Interfaces;
using BudgetTracker.DAL.Services;
using BudgetTracker.DAL.Repositories;
using BudgetTracker.DAL.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BudgetTrackerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
