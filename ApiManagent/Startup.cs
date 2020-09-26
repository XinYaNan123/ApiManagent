using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace ApiManagent
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
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                //解决返回json 大小写问题
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();

            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
             AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
             {
                 //设置 cookie 是否是只能被服务器访问，默认 true，可以设置成 false 给客户端js 脚本访问，但是有可能会造成XSS（跨站脚本攻击）。
                 options.Cookie.HttpOnly = true;

                 //cookie的有效域。默认是请求的服务器名。浏览器只会给符合的服务器发送 cookie。
                 //options.Cookie.Domain="",

                 //cookie 的名字。
                 options.Cookie.Name = "token";
                 //options.Cookie.Path

                 //置存储在 cookie 里面的认证票据的过期时间。服务端会验证加密的 ticket 的有效性。在设置了IsPersistent之后也能在 Set-Cookie 头里面返回。默认的过期时间是14天。
                 options.ExpireTimeSpan = TimeSpan.FromDays(3);
                 //设置登入页面地址
                 options.LoginPath = "/Home/index";
                 //设置了之后只有 session 的标识符会发送到客户端。当身份标识比较多的时候可以用。
                 options.SessionStore = null;

                 options.AccessDeniedPath = "/Account/AccessDenied";

             });

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
            }
            app.UseStaticFiles();

            app.UseRouting();

            //全局cookie配置
            app.UseCookiePolicy(new CookiePolicyOptions() {
                //cookie 被追加的时候调用。
                OnAppendCookie = { },

                //cookie 被删除的时候调用。
                OnDeleteCookie = { },
            });
            app.UseAuthentication();//鉴权，检测有没有登录，登录的是谁，赋值给User
            app.UseAuthorization();//就是授权，检测权限

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
