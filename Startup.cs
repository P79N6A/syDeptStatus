using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace syDeptStatus
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
            /*
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/home/login"; 
                    options.ExpireTimeSpan=new TimeSpan(0,0,10);
                });
            services.AddSingleton<UserStore>();
            */

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();

            services.AddDistributedMemoryCache();
            services.AddSession(o => { o.IdleTimeout = TimeSpan.FromMinutes(20); /*20分钟有效期*/ });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(options => {
                options.AddPolicy("any",builder=>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDirectoryBrowser();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseSession();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}