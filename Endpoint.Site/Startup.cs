using DigitalWallet.Application.Interfaces.Contexts;
using DigitalWallet.Application.Services.Transactions.Commands.MoneyTransfer;
using DigitalWallet.Application.Services.Transactions.Queries.GetUserTransactions;
using DigitalWallet.Application.Services.Users.Commands.EditUserProfile;
using DigitalWallet.Application.Services.Users.Commands.RegisterUser;
using DigitalWallet.Application.Services.Users.Commands.UserLogin;
using DigitalWallet.Application.Services.Users.Queries.GetDashboeardData;
using DigitalWallet.Application.Services.Users.Queries.GetUserProfile;
using DigitalWallet.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.Site
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString("/");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
            });

            // Services
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IUserLoginService, UserLoginService>();
            services.AddScoped<IGetDashboeardDataService, GetDashboeardDataService>();
            services.AddScoped<IGetUserProfileService, GetUserProfileService>();
            services.AddScoped<IEditUserProfileService, EditUserProfileService>();
            services.AddScoped<IGetUserTransactionsService, GetUserTransactionsService>();
            services.AddScoped<IMoneyTransferService, MoneyTransferService>();

            string contectionString = @"Data Source=DESKTOP-ABNJIE3\MSSQLSERVER2019; Initial Catalog=DigitalWalletDB; Integrated Security=True;";
            services.AddEntityFrameworkSqlServer().AddDbContext<DatabaseContext>(option => option.UseSqlServer(contectionString));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
