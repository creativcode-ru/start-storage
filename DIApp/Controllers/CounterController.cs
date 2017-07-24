using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DIApp.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DIApp.Controllers
{
    public class CounterController : Controller
    {
        private static int i = 0; // статический счетчик запросов
        ICounter Counter { get; }
        CounterService CounterService { get; }

        //Через конструктор контроллер принимает зависимости ICounter и CounterService
        public CounterController(CounterService counterService, ICounter counter)
        {
            CounterService = counterService;
            Counter = counter;
            i++;
        }

        public IActionResult Index()
        {
            return Content($"Запрос {i}; Counter: {Counter.Value}; Service: {CounterService.Counter.Value}");
        }

    }
}
