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
                //�������json ��Сд����
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();

            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
             AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
             {
                 //���� cookie �Ƿ���ֻ�ܱ����������ʣ�Ĭ�� true���������ó� false ���ͻ���js �ű����ʣ������п��ܻ����XSS����վ�ű���������
                 options.Cookie.HttpOnly = true;

                 //cookie����Ч��Ĭ��������ķ��������������ֻ������ϵķ��������� cookie��
                 //options.Cookie.Domain="",

                 //cookie �����֡�
                 options.Cookie.Name = "token";
                 //options.Cookie.Path

                 //�ô洢�� cookie �������֤Ʊ�ݵĹ���ʱ�䡣����˻���֤���ܵ� ticket ����Ч�ԡ���������IsPersistent֮��Ҳ���� Set-Cookie ͷ���淵�ء�Ĭ�ϵĹ���ʱ����14�졣
                 options.ExpireTimeSpan = TimeSpan.FromDays(3);
                 //���õ���ҳ���ַ
                 options.LoginPath = "/Home/index";
                 //������֮��ֻ�� session �ı�ʶ���ᷢ�͵��ͻ��ˡ�����ݱ�ʶ�Ƚ϶��ʱ������á�
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

            //ȫ��cookie����
            app.UseCookiePolicy(new CookiePolicyOptions() {
                //cookie ��׷�ӵ�ʱ����á�
                OnAppendCookie = { },

                //cookie ��ɾ����ʱ����á�
                OnDeleteCookie = { },
            });
            app.UseAuthentication();//��Ȩ�������û�е�¼����¼����˭����ֵ��User
            app.UseAuthorization();//������Ȩ�����Ȩ��

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
