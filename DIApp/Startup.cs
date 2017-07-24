using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// https://metanit.com/sharp/aspnet5/6.1.php 
using DIApp.Services;

namespace DIApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("myconfig.json") // подключаем файл конфигурации
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        /*Сервисы, которые создаются механизмом Depedency Injection, могут представлять один из следующих типов:
            •Transient: объект сервиса создается каждый раз, когда к нему обращается запрос. 
                        Подобная модель жизненного цикла наиболее подходит для легковесных сервисов, которые не хранят данных о состоянии
            •Scoped: для каждого запроса создается свой объект сервиса
            •Singleton: объект сервиса создается при первом обращении к нему, 
                        все последующие запросы используют один и тот же ранее созданный объект сервиса

*/
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMessageSender, EmailMessageSender>();//регистрируем собственные сервися
                                                                        //services.AddTransient<IMessageSender, SmsMessageSender>(); 
                                                                        /* Тем самым система на место объектов интерфейса IMessageSender будет передавать экземпляры класса EmailMessageSender
                                                                        */

            ////Использование фабрики сервисов
            //services.AddTransient<IMessageSender>(provider => {
            //    if (DateTime.Now.Hour >= 12) return new EmailMessageSender();
            //    else return new SmsMessageSender();
            //});



            //------------------------------------------ counter ----------------------------------------
            ////для CounterController
            ////вариант1
            //services.AddTransient<ICounter, RandomCounter>();
            //services.AddTransient<CounterService>(); // для объекта CounterService будет создаваться экземпляр этого же класса RandomCounter

            ////вариант2
            //// Метод AddScoped создает один экземпляр объекта для всего запроса.
            //services.AddScoped<ICounter, RandomCounter>();
            //services.AddScoped<CounterService>();
            ///* Теперь в рамках одно и того же запроса и контроллер и сервис CounterService будут использовать один и тот же объект RandomCounter. 
            //   При следующем запросе к контроллеру будет генерироваться новый объект RandomCounter*/

            ////вариант3
            ////AddSingleton создает один объект для всех последующих запросов, при этом объект создается только тогда, когда он непосредственно необходим.
            //services.AddSingleton<ICounter, RandomCounter>();
            //services.AddSingleton<CounterService>();

            ////вариант4
            ////Нам необязательно полагаться на механизм DI, который создает объект. Мы его можем сами создать и передать в нужный метод:
            //RandomCounter rndCounter = new RandomCounter(); //***
            //services.AddSingleton<ICounter>(rndCounter);
            //services.AddSingleton<CounterService>(new CounterService(rndCounter));


            //------------------------------------------ restriction ----------------------------------------
            // Настройка параметров и DI
            services.AddOptions(); //возможность внедрения зависимостей
            
            // создание объекта Restrictions по ключам из конфигурации
            services.Configure<Restrictions>(Configuration); //*** по данным конфигурации из свойства Configuration создается объект Restrictions
            /* В итоге в контроллер будут переданы данные конфигурации приложения через объект Restrictions.*/

            //При необходимости мы можем переопределить настройки с помощью перегрузки метода services.Configure():
            services.Configure<Restrictions>(opt =>
            {
                opt.Age = 22;
            });



            // ------ Add framework services -------
            services.AddMvc();
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ////Методе Configure позволяет напрямую получать зависимость в качестве параметра метода. Например:
            //Configure(IApplicationBuilder app, IMessageSender sender)
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync(sender.Send());
            //}); return;



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
