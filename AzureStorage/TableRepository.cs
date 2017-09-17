using AzureStorage.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorage
{
    public class TableRepository
    {

        static CloudTable TableReference(string connection, string tableName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connection);

            //Получите объект CloudTableClient, представляющий клиент службы таблиц.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Получите объект CloudTable, представляющий ссылку на требуемое имя таблицы. 
            //Метод CloudBlobClient.GetContainerReference не выполняет запрос в отношении хранилища таблиц. 
            //Ссылка возвращается независимо от существования таблицы.
            return tableClient.GetTableReference(tableName);
        }

        public static bool CreateTable(string connection, string tableName)
        {
            CloudTable table = TableReference(connection, tableName);

            //Вызовите метод CloudTable.CreateIfNotExists, чтобы создать таблицу, если она еще не создана. 
            //Метод CloudTable.CreateIfNotExists возвращает значение true, если таблица существует или успешно создана.
            //В противном случае возвращается значение false. 
            return table.CreateIfNotExists();
        }
        public static int AddCustomer(string connection, string tableName, CustomerEntity сustomer)
        {
            CloudTable table = TableReference(connection, tableName);

            //Создайте объект TableOperation, который вставляет пользовательскую сущность.
            TableOperation insertOperation = TableOperation.Insert(сustomer);

            //Выполните операцию вставки, вызвав метод CloudTable.Execute. 
            //Результат операции можно узнать, проверив значение свойства TableResult.HttpStatusCode.
            //Код состояния 2xx означает, что запрошенное клиентом действие обработано успешно. 
            //Например, успешная вставка новых сущностей выводит код состояния HTTP 204. 
            //Это значит, что операция обработана и сервер не вернул содержимое.
            TableResult result = table.Execute(insertOperation);
            return result.HttpStatusCode;
            
        }
        

        public static List<int> AddBatch(string connection, string tableName, List<CustomerEntity> сustomers)
        {
            CloudTable table = TableReference(connection, tableName);

            // Получите объект TableBatchOperation
            TableBatchOperation batchOperation = new TableBatchOperation();

            //Добавьте сущности в объект пакетной операции вставки.
            foreach (CustomerEntity customer in сustomers)
            {
                batchOperation.Insert(customer);
            }

            //Выполните пакетную операцию вставки, вызвав метод CloudTable.ExecuteBatch.
            IList<TableResult> results = table.ExecuteBatch(batchOperation);

            List<int> statusCodes = new List<int>();
            foreach (TableResult result in results)
            {
                statusCodes.Add(result.HttpStatusCode);
            }

            return statusCodes;

        }

    }
}
