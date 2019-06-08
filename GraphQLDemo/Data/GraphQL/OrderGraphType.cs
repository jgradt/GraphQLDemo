using GraphQL.Types;
using GraphQLDemo.Data.Entities;

namespace GraphQLDemo.Data.GraphQL
{
    public class OrderGraphType : ObjectGraphType<Order>
    {
        public OrderGraphType()
        {
            Name = "Order";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.OrderDate);
            Field(x => x.DeliveredDate, nullable: true);
            Field<OrderStatusGraphType>(nameof(Order.Status));
            Field(x => x.TotalDue);
            Field(x => x.Comment);
        }
    }

    public class OrderStatusGraphType : EnumerationGraphType<OrderStatus>
    {
    }
}
