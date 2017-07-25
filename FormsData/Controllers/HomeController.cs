using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FormsData.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region --------------------------------------- передача данных в представление --------------------------------
        //https://metanit.com/sharp/aspnet5/7.3.php 
        public IActionResult About()
        {

            ViewData["Message"] = "Использование ViewData";
            ViewBag.Message2 = "Использование ViewBag"; //*** ключи должны отличаться
            /* не важно, что изначально объект ViewBag не содержит никакого свойства Message, оно определяется динамически. 
               при этом свойства ViewBag могут содержать не только простые объекты типа string или int, но и сложные данные
               */
            ViewBag.Countries = new List<string> { "Бразилия", "Аргентина", "Уругвай", "Чили" };
            /* В представении может потребоваться приведение типов*/


            //TempData["Message"] = "Hello ASP.NET Core";
            /*Как и ViewData, но позволяет сохранять переданное значение в течении всего текущего запроса, за счет использоваия сессии.
              Для применения TempData нам надо подключить функциональность сессий*/

            //return View();

            //Модель представления
            List<string> countries2 = new List<string> { "Бразилия2", "Аргентина2", "Уругвай2", "Чили2" };
            return View(countries2);
            /*В самом начале представления с помощью директивы @model устанавливается модель представления. 
             Тип модели должен совпадать с типом объекта, который передается в метод View() в контроллере.*/

            //*** еще данные в представление можно передать с помощью Внедрения Зависимостей, см. проект DIApp

        }
        #endregion


        #region ----------------------------------------------------------- простейшая форма --------------------------------------------------
        //https://metanit.com/sharp/aspnet5/7.8.php
        /* Одно действие расщеплено на два метода: GET-версию, которая отдает представление с формой ввода, 
           и POST-версию, которая принимает введенные в эту форму данные
         */
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Login(string login, string password)
        //{
        //    //string authData = $"Логин: {login}   Пароль: {password}";
        //    /* Чтобы инфраструктура MVC могла автоматически связать данные из формы с параметрами метода, 
        //       значения атрибутов [name] у полей формы совпадают с именами параметров.
        //       <input type="text" name="login" />>
        //       <input type="text" name="password" />
        //     */

            
        //    string authData = $"Login: {login}   Password: {password}   Age: {age}  Comment: {comment}";
        //    return Content(authData); //контроллер может непосредственно выводить данные, без представления
        //}

        //используем поля с типами password, int, textarea
        [HttpPost]
        public IActionResult Login(string login, string password, int age, bool isMarried,
            string comment, string color, string phone, string[] phones)
        {
            string result = "";
            foreach (string p in phones)
            {
                result += p;
                result += ";";
            }


            string authData = $"Login: {login}   Password: {password} Женат: {isMarried}  Age: {age}  Comment: {comment} Color: {color} Телефон: {phone}, Тел. {result}";
            return Content(authData);
        }


        #endregion


        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
