using ApplicantPersonalAccount.API;
using ApplicantPersonalAccount.Application;
using ApplicantPersonalAccount.Application.Implementations;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Repositories;
using ApplicantPersonalAccount.Persistence.Repositories.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var usersConnection = builder.Configuration.GetConnectionString("UsersConnection");

builder.Services.AddDbContext<UserDataContext>(options => options.UseNpgsql(usersConnection));

// services
builder.Services.AddTransient<IAuthorizationService, AuthorizationServiceImpl>();
builder.Services.AddTransient<ITokenService, TokenServiceImpl>();

// repositories
builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var serviceScope = app.Services.CreateScope();

var context = serviceScope.ServiceProvider.GetService<UserDataContext>();

context?.Database.Migrate();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
