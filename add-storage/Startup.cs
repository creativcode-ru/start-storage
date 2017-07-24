using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using add_storage.Repositories;

namespace add_storage
{
    public class Startup
    {
        //Как правило, установка конфигурации производится в кострукторе класа Startup
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //читаем настройки конфирурации
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*Поскольку ядро ASP.NET MVC обеспечивает иньекцию по умолчанию, 
             * нам необходимо зарегистрировать класс TableClientOperationsService в контейнере, используя IServiceCollection. 
             * Нам нужно зарегистрировать IConfigurationRoot в контейнере, чтобы он мог обеспечить доступ к ключам из файлов JSON в проекте*/
            services.AddSingleton(typeof(ITableRepositories), typeof(TableClientOperationsService));
            services.AddSingleton<IConfigurationRoot>(Configuration);
            /* •Singleton: объект сервиса создается при первом обращении к нему, все последующие запросы используют один и тот же ранее созданный объект сервиса
             */

            // Add framework services.
            services.AddMvc();
        }

        /**/

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
