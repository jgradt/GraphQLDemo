using System;
using System.Collections.Generic;

namespace GraphQLDemo.Data.Entities
{
    public class Supplier : IDataEntity
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public List<Product> Products { get; set; }
    }
}
