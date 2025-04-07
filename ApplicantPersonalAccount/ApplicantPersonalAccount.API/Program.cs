using ApplicantPersonalAccount.API.Setup;

var builder = WebApplication.CreateBuilder(args);

SetupAspNet.AddAspNet(builder);
SetupSwagger.AddSwagger(builder);
SetupDatabases.AddDatabases(builder);
SetupServises.AddServises(builder.Services);
SetupRepositories.AddRepositories(builder.Services);
SetupAuth.AddAuth(builder);

var app = builder.Build();

SetupSwagger.UseSwagger(app);
SetupDatabases.RunMigrations(app);
SetupAuth.UseAuth(app);
SetupAspNet.UseAspNet(app);

app.Run();
