using ApplicantPersonalAccount.Staff.Setup;

var builder = WebApplication.CreateBuilder(args);

SetupMVC.AddMVC(builder);
SetupAuth.AddAuth(builder);
SetupServices.AddServices(builder.Services);
SetupLogs.AddLogs(builder);

var app = builder.Build();

SetupLogs.ApplyLogs(app);
SetupMVC.UseMVC(app);
SetupAuth.UseAuth(app);
SetupRouting.UseRouting(app);

app.Run();
