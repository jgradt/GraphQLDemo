using GraphQL.Types;
using GraphQLDemo.Data.Repositories;

namespace GraphQLDemo.Data.GraphQL
{

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

}
