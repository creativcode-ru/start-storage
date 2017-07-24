using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DIApp.Services;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.DependencyInjection;

namespace DIApp.Controllers
{
    public class HomeController : Controller
    {
        //при создании класса регистрируем сервис
        //что именно передавать в переменную IMessageSender sender определяется в StartUp.ConfigureServices()
        IMessageSender _sender;
        /* Cистема внедрения зависимостей использует конструкторы классов для передачи всех зависимостей/
           ASP.NET Core поддерживает наличие только одного конструктора, который получает зависимость/

           Когда приходит запрос к контроллеру, инфраструктура MVC вызывает провайдер сервисов для создания объекта HomeController. 
           Провайдер сервисов проверят конструктор класса HomeController на наличие зависимостей. 
           Затем создает объекты для всех используемых зависимостей и передает их в конструкторо HomeController для создания объекта контроллера, 
           который затем обрабатывает запрос.
         */
        public HomeController(IMessageSender sender)
        {
            _sender = sender;
        }
        /* В данном случае процесс установки зависимостей будет выглядеть следующим образом:
                1.Приложение получает запрос к методу контроллера HomeController
                2.Фреймворк MVC обращается к провайдеру сервисов для создания объекта контроллера HomeController
                3.Провайдер сервисов смотрит на конструктор класса HomeController и видит, что там имеется зависимость от интерфейса IMessageSender
                4.Провайдер сервисов среди зарегистрированных зависимостей ищет класс, который представляет реализацию интерфейса IMessageSender
                5.Если нужная зависимость найдена, то провайдер сервисов создает объект класса, который реализует интерфейс IMessageSender
                6.Затем провайдер сервисов создает объект HomeController, передавая в его конструктор ранее созданную реализацию IMessageSender
                7.В конце провайдер сервисов возвращает созданный объект HomeController инфраструктуре MVC, которая использует контроллер для обработки запроса
            
            Однако конструкторы - не единственное место для передачи зависимостей.
         */

        //public IActionResult Index()
        //{
        //    return Content(_sender.Send());
        //}

        //------------------------- 2 ------------------------
        /*Иногда зависимость используется только в одном методе. 
          И в этом случае нет необходимости передавать ее в контроллер, поскольку она напрямую может быть внедрена в сам метод, который ее использует. 
           Для передачи зависимости в метод применяется атрибут [FromServices]*/
        //public IActionResult Index([FromServices] IHostingEnvironment env)
        //{
        //    return Content(env.ContentRootPath);
        //}

        //------------------------- 3 ------------------------
        /*Объект HttpContext.RequestServices предоставляет доступ к всем внедренным зависимостям с помощью своих методов:
         * 
            •GetService<service>(): использует провайдер сервисов для создания объекта, который представляет тип service. 
                                    В случае если в провайдере сервисов для данного сервиса не установлена зависимость, то возвращает значение null
            •GetRequiredService<service>(): использует провайдер сервисов для создания объекта, который представляет тип service. 
                                    В случае если в провайдере сервисов для данного сервиса не установлена зависимость, то генерирует исключение
         
            (using Microsoft.Extensions.DependencyInjection;)*/
        public IActionResult Index()
        {
            // получаем зависимость для IHostingEnvironment
            var env = HttpContext.RequestServices.GetService<IHostingEnvironment>();
            //или var env = HttpContext.RequestServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            return Content(env.ContentRootPath);
        }

        //пример внедрения зависимостей сразу в представление
        public IActionResult Msg()
        {
            return View();
        }

        /*
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        */
    }
}
