using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//
using Microsoft.WindowsAzure.Storage.Table;

namespace add_storage.Models
{
    public class Book : TableEntity
    {
        public Book()
        {
        }

        public Book(int bookid, string publisher)
        {
            this.RowKey = bookid.ToString();
            this.PartitionKey = publisher;
        }

        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public string Publisher { get; set; }
    }

}
