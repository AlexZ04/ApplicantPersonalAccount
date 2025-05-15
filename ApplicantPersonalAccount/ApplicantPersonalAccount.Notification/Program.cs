using ApplicantPersonalAccount.Notification.Setup;

var builder = WebApplication.CreateBuilder(args);

SetupAspNet.AddAspNet(builder);
SetupSwagger.AddSwagger(builder);
SetupServices.AddServices(builder.Services);
SetupDatabases.AddDatabases(builder);
SetupRepositories.AddRepositories(builder.Services);

var app = builder.Build();

SetupSwagger.UseSwagger(app);
SetupDatabases.RunMigrations(app);
SetupAspNet.UseAspNet(app);

app.Run();
