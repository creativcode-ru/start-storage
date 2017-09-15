using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AzureStorage;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace storage_core.Controllers
{
    public class TablesController : Controller
    {

        #region * * * * * ИНЪЕКЦИЯ КОНФИГУРАЦИИ * * * * *
        IConfiguration _configuration;
        public TablesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult CreateTable()
        {

            //получите объект CloudStorageAccount, который представляет сведения об учетной записи хранения. 
            //Используйте следующий фрагмент кода, чтобы получить строку подключения к хранилищу и сведения об учетной записи хранения из конфигурации службы Azure.

            string c = _configuration.GetConnectionString("start1storage_AzureStorageConnectionString");
            string tableName = "TestTable";
            ViewBag.Success = TableRepository.CreateTable(c, tableName);
            ViewBag.TableName = tableName;

            return View();
        }

    }
}
