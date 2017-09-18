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

        #region ----------------------------- Создать / Удалить таблицу ---------------------------------------------------------------------
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
        #endregion

        public static bool CreateTable(string connection, string tableName)
        {
            CloudTable table = TableReference(connection, tableName);

            //Вызовите метод CloudTable.CreateIfNotExists, чтобы создать таблицу, если она еще не создана. 
            //Метод CloudTable.CreateIfNotExists возвращает значение true, если таблица существует или успешно создана.
            //В противном случае возвращается значение false. 
            return table.CreateIfNotExists();
        }

        #region ---------------------------------------------- Запись в таблицу ------------------------------------------------------------

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

        #endregion

        #region ------------------------------------------------ Чтение / удаление из таблицы --------------------------------------------------------------
        public static CustomerEntity ReadCustomer(string connection, string tableName, string partKey, string rowKey)
        {
            CloudTable table = TableReference(connection, tableName);

            //Создайте объект операции извлечения, который принимает объект сущности, производный от TableEntity.
            //Первый параметр — ключ раздела partitionKey, а второй — ключ строки rowKey.
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(partKey, rowKey);

            //Выполните операцию извлечения.
            CustomerEntity сustomer = null;
            сustomer = table.Execute(retrieveOperation).Result as CustomerEntity;

            return сustomer;
        }



        public static List<CustomerEntity> ReadPartition(string connection, string tableName, string partKey)
        {
            CloudTable table = TableReference(connection, tableName);

            //Создайте экземпляр объекта TableQuery, поместив запрос в предложение Where.Используя класс CustomerEntity и данные, 
            //представленные в разделе Добавление пакета сущностей в таблицу, 
            //следующий фрагмент кода направляет запрос к таблице на получение всех сущностей, 
            //свойство PartitionKey(фамилия пользователя) которых имеет значение Smith.
            TableQuery<CustomerEntity> query =  new TableQuery<CustomerEntity>()
                                        .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partKey));

            //В цикле вызовите метод CloudTable.ExecuteQuerySegmented, передавая ему экземпляр объекта запроса, 
            //который вы создали на предыдущем шаге.
            //Метод CloudTable.ExecuteQuerySegmented возвращает объект TableContinuationToken. 
            //Этот объект будет иметь значение null, когда нет больше сущностей для извлечения. 
            //В цикле используйте другой цикл для перечисления возвращаемых сущностей.
            //В следующем примере кода каждая возвращенная сущность добавляется в список. 
            //Когда цикл завершается, список передается в представление для отображения:
            List<CustomerEntity> customers = new List<CustomerEntity>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<CustomerEntity> resultSegment = table.ExecuteQuerySegmented(query, token);
                token = resultSegment.ContinuationToken;

                foreach (CustomerEntity customer in resultSegment.Results)
                {
                    customers.Add(customer);
                }
            } while (token != null);

            return customers;
        }

        public static CustomerEntity DeleteCustomer(string connection, string tableName, string partKey, string rowKey)
        {
            CloudTable table = TableReference(connection, tableName);

            //Создайте объект операции удаления, который принимает объект сущности, производный от TableEntity.
            //В этом случае используется класс CustomerEntity и данные, представленные в разделе Добавление пакета сущностей в таблицу. 
            //ETag сущности должно быть присвоено допустимое значение.
            TableOperation deleteOperation = TableOperation.Delete(new CustomerEntity(partKey, rowKey) { ETag = "*" });

            //Выполните операцию удаления. Одновременно извлекается удаленная сущьность. 
            CustomerEntity сustomer = null;
            сustomer = table.Execute(deleteOperation).Result as CustomerEntity;

            return сustomer;

        }

        #endregion

    }
}
