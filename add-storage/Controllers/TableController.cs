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

    /*Класс контроллера инъецируется ITableRepositories, который зарегистрирован в Injection Dependency. 
      Это обеспечивает экземпляр класса TableClientOperationsService. 
      Метод действия Index () обращается к методу GetBooks () класса TableClientOperationsService для чтения всех книг. 
      Метод Create () с атрибутом HttpPost используется для создания новой книги в хранилище 
      с использованием метода CreateBook () класса TableClientOperationsService
    */

    public class TableController : Controller
    {

        ITableRepositories serv;
        public TableController(ITableRepositories s)
        {
            serv = s;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var books = serv.GetBooks();
            return View(books);
        }

        public IActionResult Create()
        {
            var book = new Book();
            return View(book);
        }

        [HttpPost]
        public IActionResult Create(Book bk)
        {
            serv.CreateBook(bk);
            return RedirectToAction("Index");
        }

    }
}
