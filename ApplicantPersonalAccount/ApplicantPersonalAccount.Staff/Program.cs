using ApplicantPersonalAccount.Staff.Setup;

var builder = WebApplication.CreateBuilder(args);

SetupMVC.AddMVC(builder);
SetupAuth.AddAuth(builder);
SetupServices.AddServices(builder.Services);

var app = builder.Build();

SetupMVC.UseMVC(app);
SetupAuth.UseAuth(app);
SetupRouting.UseRouting(app);

app.Run();
