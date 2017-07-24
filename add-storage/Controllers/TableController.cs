using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using add_storage.Repositories;
using add_storage.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace add_storage.Controllers
{

    public class TableController : Controller
    {

        /*Контроллер содержит зависимость ITableRepositories,
         а в SartUp указано, что ITableRepositories должен передать TableClientOperationsService как Singleton
        */
        ITableRepositories serv;
        public TableController(ITableRepositories s)
        {
            serv = s;
        }
        /* * * * * подробнее в проекте DIApp * * * * */

        // GET: /<controller>/
        public IActionResult Index()
        {
            var books = serv.GetBooks(); //TableClientOperationsService.GetBooks - чтение всех книг
            return View(books);
        }

        //отображение пустой формы для создания книг
        public IActionResult Create()
        {
            var book = new Book();
            return View(book);
        }

        //создания новой книги в хранилище, получены данные из формы
        [HttpPost]
        public IActionResult Create(Book bk)
        {
            serv.CreateBook(bk);
            return RedirectToAction("Index");
        }

    }
}
