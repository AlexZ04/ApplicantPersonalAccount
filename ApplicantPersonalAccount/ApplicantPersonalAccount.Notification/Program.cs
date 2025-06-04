using ApplicantPersonalAccount.Notification.Setup;

var builder = WebApplication.CreateBuilder(args);

SetupAspNet.AddAspNet(builder);
SetupSwagger.AddSwagger(builder);
SetupServices.AddServices(builder.Services);
SetupDatabases.AddDatabases(builder);
SetupRepositories.AddRepositories(builder.Services);
SetupLogs.AddLogs(builder); 

var app = builder.Build();

SetupLogs.ApplyLogs(app);
SetupSwagger.UseSwagger(app);
SetupDatabases.RunMigrations(app);
SetupAspNet.UseAspNet(app);

app.Run();
