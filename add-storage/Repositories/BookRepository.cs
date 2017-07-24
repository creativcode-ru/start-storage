using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

using add_storage.Models;

//http://www.dotnetcurry.com/visualstudio/1328/visual-studio-connected-services-aspnet-core-azure-storage

namespace add_storage.Repositories
{
    //1.
    /*Интерфейс ITableRepositories содержит описание методов для выполнения операций в таблице Book.*/
    public interface ITableRepositories
    {
        void CreateBook(Book bk);
        List<Book> GetBooks();
        Book GetBook(string pKey, string rKey);
    }

    public class TableClientOperationsService : ITableRepositories
    {
        //2.
        /*Класс TableClientOperationsService реализует интерфейс ITableRepositories. 
         Объявлен объект CloudStorageAccount для доступа к учетной записи хранилища на основе строки подключения.*/
        CloudStorageAccount storageAccount;
        //3.
        /*Объект CloudTableClient будет использоваться для выполнения операций Create и Read.*/
        CloudTableClient tableClient;
        //4.
        /* Ссылка IConfigurationRoot будет использоваться для чтения файлов JSON в проекте. 
         В нашем проекте у нас есть appsettings.json, config.json, который содержит пары ключ / значение для параметров проекта*/
        IConfigurationRoot configs;

        //5.
        /* Конструктор класса играет очень важную роль. 
          Он вводится объектом IConfigurationRoot, чтобы он мог читать ключи кокфигурации. 
          IConfigurationRoot считывает строку подключения хранилища на основе ключа, 
          то есть ConnectionStrings:start1storage_AzureStorageConnectionString. 

          Код создает экземпляр учетной записи хранилища на основе строки подключения. 
          Код создает экземпляр клиента таблицы и создает таблицу имени Book, если она еще не существует.*/
        public TableClientOperationsService(IConfigurationRoot c)
        {
            this.configs = c;
            var connStr = this.configs.GetSection("ConnectionStrings:start1storage_AzureStorageConnectionString"); //подробнее https://metanit.com/sharp/aspnet5/2.17.php
            storageAccount = CloudStorageAccount.Parse(connStr.Value);//учетная запись хранилища

            tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Book"); //экземпляр клиента таблицы
            table.CreateIfNotExists();//создает таблицу Book, если она еще не существует
        }

        //6.
        /* Метод CreateBook добавляет новую запись в таблицу*/
        public void CreateBook(Book bk)
        {
            Random rnd = new Random();
            bk.BookId = rnd.Next(100);
            bk.RowKey = bk.BookId.ToString();
            bk.PartitionKey = bk.Publisher;
            CloudTable table = tableClient.GetTableReference("Book");
            TableOperation insertOperation = TableOperation.Insert(bk);
            table.Execute(insertOperation);
        }
        //7.
        /* Метод GetBook извлекает книгу на основе ключа таблицы RowKey и раздела*/
        public Book GetBook(string pKey, string rKey)
        {
            Book entity = null;
            CloudTable table = tableClient.GetTableReference("Book");
            TableOperation tableOperation = TableOperation.Retrieve<Book>(pKey, rKey);
            entity = table.Execute(tableOperation).Result as Book;
            return entity;
        }
        //8.
        /* Метод GetBooks читает все книги из таблицы книг*/
        public List<Book> GetBooks()
        {
            List<Book> Books = new List<Book>();
            CloudTable table = tableClient.GetTableReference("Book");

            TableQuery<Book> query = new TableQuery<Book>();

            foreach (var item in table.ExecuteQuery(query))
            {
                Books.Add(new Book()
                {
                    BookId = item.BookId,
                    BookName = item.BookName,
                    Author = item.Author,
                    Publisher = item.Publisher,
                    Price = item.Price
                });
            }

            return Books;
        }
    }
}

