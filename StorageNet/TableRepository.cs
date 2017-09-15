using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace StogageNet
{
    public class TableRepository
    {
        public static bool CreateTable(string connection, string tableName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connection);

            //Получите объект CloudTableClient, представляющий клиент службы таблиц.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Получите объект CloudTable, представляющий ссылку на требуемое имя таблицы. 
            //Метод CloudBlobClient.GetContainerReference не выполняет запрос в отношении хранилища таблиц. 
            //Ссылка возвращается независимо от существования таблицы.
            CloudTable table = tableClient.GetTableReference(tableName);

            //Вызовите метод CloudTable.CreateIfNotExists, чтобы создать таблицу, если она еще не создана. 
            //Метод CloudTable.CreateIfNotExists возвращает значение true, если таблица существует или успешно создана.
            //В противном случае возвращается значение false. 
            bool result =  table.CreateIfNotExists();

            return false;
        }
    }
}
