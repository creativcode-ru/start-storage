using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace storage_net.Controllers
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
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(c);

            //Получите объект CloudTableClient, представляющий клиент службы таблиц.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Получите объект CloudTable, представляющий ссылку на требуемое имя таблицы. 
            //Метод CloudBlobClient.GetContainerReference не выполняет запрос в отношении хранилища таблиц. 
            //Ссылка возвращается независимо от существования таблицы.
            CloudTable table = tableClient.GetTableReference("TestTable");

            //Вызовите метод CloudTable.CreateIfNotExists, чтобы создать таблицу, если она еще не создана. 
            //Метод CloudTable.CreateIfNotExists возвращает значение true, если таблица существует или успешно создана.
            //В противном случае возвращается значение false. 
            ViewBag.Success = table.CreateIfNotExists();
            ViewBag.TableName = table.Name;

            return View();
        }

    }
}
