using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

//
using Microsoft.Extensions.Options;
using DIApp.Services;

namespace DIApp.Controllers
{
    //чтобы передать конфигурационные настройки, мы можем использовать сервис IOptions<TOptions>.
    //в StartUp должен быть подключен фйл конфмгурации
    //// и в ConfigureServices надо настроить DI для IOptions<Restrictions>
    public class RestrictionsController : Controller
    {

        public RestrictionsController(IOptions<Restrictions> o)
        {
            Restrictions = o.Value;
            // в качестве альтернативы мы можем обращаться к отдельным свойствам
            //Restrictions.Age = o.Value.Age;
            ////Restrictions.Countries = o.Value.Countries;
        }
        public Restrictions Restrictions { get; }

        public IActionResult Index()
        {
            string ageInfo = $"Age:{Restrictions.Age}";
            return Content(ageInfo);
        }
    }
}
