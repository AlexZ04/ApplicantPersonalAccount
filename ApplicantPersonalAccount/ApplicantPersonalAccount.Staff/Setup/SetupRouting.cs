namespace ApplicantPersonalAccount.Staff.Setup
{
    public class SetupRouting
    {
        public static void UseRouting(WebApplication app)
        {
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
