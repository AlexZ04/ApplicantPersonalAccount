using ApplicantPersonalAccount.Staff.Setup;

var builder = WebApplication.CreateBuilder(args);

SetupMVC.AddMVC(builder);

var app = builder.Build();

SetupMVC.UseMVC(app);
SetupRouting.UseRouting(app);

app.Run();
