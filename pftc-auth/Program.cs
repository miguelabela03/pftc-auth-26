using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace pftc_auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(
                Options =>
                {
                    Options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    Options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                }
             ).AddCookie()
             .AddGoogle(GoogleDefaults.AuthenticationScheme, Options =>
                 {
                     Options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                     Options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                     Options.Scope.Add("profile");
                     Options.Events.OnCreatingTicket = (context) =>
                     {
                         var email = context.User.GetProperty("email").GetString();
                         var picture = context.User.GetProperty("picture").GetString();

                         context.Identity.AddClaim(new Claim("email", email));
                         context.Identity.AddClaim(new Claim("picture", picture));

                         return Task.CompletedTask;
                     };
                 }
             );

            builder.Services.AddAuthorization();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
