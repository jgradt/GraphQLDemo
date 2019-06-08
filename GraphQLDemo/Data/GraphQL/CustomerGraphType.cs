using GraphQL;
using GraphQL.Types;
using GraphQLDemo.Data.Repositories;
using GraphQLDemo.Data.Entities;

namespace GraphQLDemo.Data.GraphQL
{
    public class CustomerGraphType : ObjectGraphType<Customer>
    {
        public CustomerGraphType(IOrderRepository orderRepository)
        {
            Name = "Customer";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.FirstName);
            Field(x => x.LastName);
            Field(x => x.Email);

            FieldAsync<ListGraphType<OrderGraphType>>(
              "orders",

              arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "count" }),

              resolve: async context =>
              {
                  var numItems = context.GetArgument<int>("count");
                  numItems = numItems > 0 ? numItems : 10;

                  var data = await orderRepository.GetPagedAsync(0, numItems, filter: o => o.CustomerId == context.Source.Id);

                  return data.Items;
              }
            );
        }
    }

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

    public class OrderStatusGraphType : EnumerationGraphType<GraphQLDemo.Data.Entities.OrderStatus>
    {
    }

    public class GraphQLQuery : ObjectGraphType
    {
        public GraphQLQuery(ICustomerRepository customerRepository, 
            IOrderRepository orderRepository)
        {
            FieldAsync<CustomerGraphType>(
              "customer",

              arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "id" }),

              resolve: async context =>
              {
                  var id = context.GetArgument<int>("id");

                  var data = await customerRepository.GetByIdAsync(id);

                  return data;
              }
            );

            FieldAsync<OrderGraphType>(
              "order",

              arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "id" }),

              resolve: async context =>
              {
                  var id = context.GetArgument<int>("id");

                  var data = await orderRepository.GetByIdAsync(id);

                  return data;
              }
            );

        }
    }

    public class QuerySchema : Schema
    {
        public QuerySchema(IDependencyResolver resolver)
          : base(resolver)
        {
            Query = resolver.Resolve<GraphQLQuery>();
            //Mutation = resolver.Resolve<GraphQLMutation>();
        }
    }

    public class GraphQLUserContext
    {
    }

}
