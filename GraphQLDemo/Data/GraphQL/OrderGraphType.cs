using GraphQL.Types;
using GraphQLDemo.Data.Entities;
using GraphQLDemo.Data.Repositories;

namespace GraphQLDemo.Data.GraphQL
{
    public class OrderGraphType : ObjectGraphType<Order>
    {
        public OrderGraphType(ICustomerRepository customerRepository)
        {
            Name = "Order";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.OrderDate);
            Field(x => x.DeliveredDate, nullable: true);
            Field<OrderStatusGraphType>(nameof(Order.Status));
            Field(x => x.TotalDue);
            Field(x => x.Comment);

            FieldAsync<CustomerGraphType>(
              "customer",

              resolve: async context =>
              {
                  var numItems = context.GetArgument<int>("count");
                  numItems = numItems > 0 ? numItems : 10;

                  var data = await customerRepository.GetByIdAsync(context.Source.CustomerId);

                  return data;
              }
            );
        }
    }

    public class OrderStatusGraphType : EnumerationGraphType<OrderStatus>
    {
    }
}
