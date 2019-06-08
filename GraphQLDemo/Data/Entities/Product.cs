using System;

namespace GraphQLDemo.Data.Entities
{
    public class Product : IDataEntity
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public int UnitPrice { get; set; }
        public bool Discontinued { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public Supplier Supplier { get; set; }
    }
}
