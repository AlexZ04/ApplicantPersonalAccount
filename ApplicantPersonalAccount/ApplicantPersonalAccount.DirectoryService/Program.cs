using ApplicantPersonalAccount.DirectoryService.Setup;

var builder = WebApplication.CreateBuilder(args);

SetupAspNet.AddAspNet(builder);
SetupSwagger.AddSwagger(builder);
SetupDatabases.AddDatabases(builder);
SetupServices.AddServices(builder.Services);
SetupRepositories.AddRepositories(builder.Services);
SetupAuth.AddAuth(builder);
SetupLogs.AddLogs(builder); 

var app = builder.Build();

SetupLogs.ApplyLogs(app);
SetupSwagger.UseSwagger(app);
SetupDatabases.RunMigrations(app);
SetupAuth.UseAuth(app);
SetupAspNet.UseAspNet(app);

app.Run();