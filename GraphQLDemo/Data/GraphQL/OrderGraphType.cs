using GraphQL.Types;
using GraphQLDemo.Data.Entities;
using GraphQLDemo.Data.Repositories;

namespace GraphQLDemo.Data.GraphQL
{
    public class OrderGraphType : ObjectGraphType<Order>
    {
        public OrderGraphType(ICustomerRepository customerRepository, IOrderDetailRepository orderDetailRepository)
        {
            Name = "Order";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.OrderDate);
            Field(x => x.DeliveredDate, nullable: true);
            Field<OrderStatusGraphType>(nameof(Order.Status));
            Field(x => x.TotalDue);
            Field(x => x.Comment);

            FieldAsync< NonNullGraphType<CustomerGraphType>>(
              "customer",

              resolve: async context =>
              {
                  var data = await customerRepository.GetByIdAsync(context.Source.CustomerId);

                  return data;
              }
            );

            FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<OrderDetailsGraphType>>>>(
              "details",

              arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "limit" }),

              resolve: async context =>
              {
                  var numItems = context.GetArgument<int>("limit");
                  numItems = numItems > 0 ? numItems : 10;

                  var data = await orderDetailRepository.GetPagedAsync(0, numItems, filter: o => o.OrderId == context.Source.Id);

                  return data.Items;
              }
            );
        }
    }

    public class OrderStatusGraphType : EnumerationGraphType<OrderStatus>
    {
    }
}
