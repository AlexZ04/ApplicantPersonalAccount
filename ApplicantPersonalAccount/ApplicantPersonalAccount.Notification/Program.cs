using ApplicantPersonalAccount.Notification.Setup;

var builder = WebApplication.CreateBuilder(args);

SetupAspNet.AddAspNet(builder);
SetupSwagger.AddSwagger(builder);
SetupServices.AddServices(builder.Services);
SetupRepositories.AddRepositories(builder.Services);

var app = builder.Build();

SetupSwagger.UseSwagger(app);
SetupAspNet.UseAspNet(app);

app.Run();
