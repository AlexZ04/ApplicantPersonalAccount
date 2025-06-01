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
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseExceptionHandler("/Home/Error");
            
            app.UseAuthorization();
        }
    }
}
