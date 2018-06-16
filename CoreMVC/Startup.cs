using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 运行时调用，向容器添加服务
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.Configure<CookiePolicyOptions>(options =>
        //    {
        //        //lambda GDPR 《通用数据保护条例》 是否需要用户同意非必要的Cookie 
        //        options.CheckConsentNeeded = context => true;
        //        options.MinimumSameSitePolicy = SameSiteMode.None;
        //    });


        //    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


        //    /*
        //     * 注入服务
        //       Transient(瞬时的)： 每次请求时都会创建的瞬时生命周期服务。这个生命周期最适合轻量级，无状态的服务。
        //       Scoped(作用域的)：在同作用域,服务每个请求只创建一次。
        //       Singleton(唯一的)：全局只创建一次,第一次被请求的时候被创建,然后就一直使用这一个.
        //     */
        //    services.AddTransient<IOC.ITestService, IOC.TestService>();
        //    services.AddScoped<IOC.ITestService2, IOC.TestService2>();
        //    services.AddSingleton<IOC.ITestService3, IOC.TestService3>();

        //    //目录浏览服务
        //    //services.AddDirectoryBrowser();
        //}


        // 运行时调用，向容器添加服务
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //采用属性注入需替换控制器所有者
            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            services.AddMvc(); 
            services.AddDirectoryBrowser();

            /*
             * 配置IServiceProvider从Autofac容器中解析（设置一个有效的Autofac服务适配器）
             Autofac
             Autofac.Extensions.DependencyInjection(这个包扩展了一些微软提供服务的类.来方便替换autofac)
            */
            var containerBuilder = new ContainerBuilder();
            
            //模块化注入
            containerBuilder.RegisterModule<IOC.DefaultModule>();

            //采用属性注入控制器
            //containerBuilder.RegisterType<Controllers.AutoDIController>().PropertiesAutowired();

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }



        // 运行时调用，配置HTTP请求管道
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
