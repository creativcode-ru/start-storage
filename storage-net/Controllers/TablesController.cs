using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;



using Microsoft.Extensions.Configuration;
using AzureStorage;
using AzureStorage.Models;


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


        string _tableName = "TestTable";
        public ActionResult CreateTable()
        {

            //получите объект CloudStorageAccount, который представляет сведения об учетной записи хранения. 
            //Используйте следующий фрагмент кода, чтобы получить строку подключения к хранилищу и сведения об учетной записи хранения из конфигурации службы Azure.

            string c = _configuration.GetConnectionString("start1storage_AzureStorageConnectionString");
            string tableName = "TestTable2";
            ViewBag.Success =TableRepository.CreateTable(c, _tableName);
            ViewBag.TableName = _tableName;

            return View();
        }

        public ActionResult AddEntity()
        {
            //Создайте и инициализируйте класс CustomerEntity.
            CustomerEntity customer = new CustomerEntity("Harp", "Walter");
            customer.Email = "Walter@contoso.com";

            string c = _configuration.GetConnectionString("start1storage_AzureStorageConnectionString");
            ViewBag.TableName = _tableName;
            ViewBag.Result = TableRepository.AddCustomer(c, _tableName, customer);
            return View();
        }

        public ActionResult AddBatch()
        {

            List<CustomerEntity> customers = new List<CustomerEntity>();
            CustomerEntity customer1 = new CustomerEntity("Smith", "Jeff");
            customer1.Email = "Jeff@contoso.com";
            customers.Add(customer1);

            CustomerEntity customer2 = new CustomerEntity("Smith", "Ben");
            customer2.Email = "Ben@contoso.com";
            customers.Add(customer2);


            string c = _configuration.GetConnectionString("start1storage_AzureStorageConnectionString");
            ViewBag.TableName = _tableName;
            List<int> results= TableRepository.AddBatch(c, _tableName, customers);

            return View(results);
        }


    }
}
