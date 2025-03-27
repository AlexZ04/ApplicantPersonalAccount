using ApplicantPersonalAccount.Application;
using ApplicantPersonalAccount.Application.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var usersConnection = builder.Configuration.GetConnectionString("UsersConnection");

//builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connection));
builder.Services.AddTransient<IAuthorizationService, AuthorizationServiceImpl>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//using var serviceScope = app.Services.CreateScope();

//var context = serviceScope.ServiceProvider.GetService<DataContext>();

//context?.Database.Migrate();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
