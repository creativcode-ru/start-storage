using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


// Конфигурация Core: https://metanit.com/sharp/aspnet5/2.6.php
using Microsoft.Extensions.Configuration; //базовая конфигурация в памяти

/*Для задания конфигурации используются различные источники - данные в памяти, 
 файлы JSON, XML, INI. При этом мы можем испльзовать сразу несколько источников. */

namespace app_config
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            // строитель конфигурации, несколько источников
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath);
            builder.AddJsonFile("confone.json"); //, optional: false, reloadOnChange: true
            builder.AddJsonFile("conftwo.json");
            builder.AddEnvironmentVariables();
            builder.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        {"firstname", "Tom"},
                        {"age", "31"}
                    });

      

            

            // создаем конфигурацию
            AppConfiguration = builder.Build();
        }
        // свойство, которое будет хранить конфигурацию
        public IConfiguration AppConfiguration { get; set; }



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            /*
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            */

            /*Поскольку это обычный словарь, то его можно динамически менять (раскоментируйте две строчки ниже)*/
            //AppConfiguration["firstname"] = "alice";
            AppConfiguration["lastname"] = "simpson"; //и пополнять другими ключами

            //var color = AppConfiguration["color"]; 

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(string.Format("firstname {0}, lastname {1}, color:{2}, namespace.class.method:{3}"
                    , AppConfiguration["firstname"], AppConfiguration["lastname"], AppConfiguration["color"]
                    , AppConfiguration["namespace:class:method"]));
            
            });

            // Работа с конфигурацией https://metanit.com/sharp/aspnet5/2.17.php
        }
    }
}
