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

            FieldAsync<ListGraphType<NonNullGraphType<OrderGraphType>>>(
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

}
