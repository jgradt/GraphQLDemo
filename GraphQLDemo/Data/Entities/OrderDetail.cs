using System;

namespace GraphQLDemo.Data.Entities
{
    public class OrderDetail : IDataEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
