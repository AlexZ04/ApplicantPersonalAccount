using ApplicantPersonalAccount.Staff.Domain.Infrascructure;

namespace ApplicantPersonalAccount.Staff.Setup
{
    public class SetupMVC
    {
        public static void AddMVC(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        public static void UseMVC(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<UpdateTokenMiddleware>();
        }
    }
}
