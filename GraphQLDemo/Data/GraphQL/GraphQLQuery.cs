using GraphQL;
using GraphQL.Types;
using GraphQLDemo.Data.Repositories;

namespace GraphQLDemo.Data.GraphQL
{

    public class GraphQLQuery : ObjectGraphType
    {
        public GraphQLQuery(ICustomerRepository customerRepository,
            IOrderRepository orderRepository, IProductRepository productRepository,
            ISupplierRepository supplierRepository)
        {
            FieldAsync<CustomerGraphType>(
              "customer",

              arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "id" }),

              resolve: async context =>
              {
                  var id = context.GetArgument<int>("id");
                  if (id <= 0)
                  {
                      context.Errors.Add(new ExecutionError("valid id >= 0 is required"));
                      return null;
                  }

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
                  if (id <= 0)
                  {
                      context.Errors.Add(new ExecutionError("valid id >= 0 is required"));
                      return null;
                  }

                  var data = await orderRepository.GetByIdAsync(id);

                  return data;
              }
            );

            FieldAsync<ProductGraphType>(
              "product",

              arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "id" }),

              resolve: async context =>
              {
                  var id = context.GetArgument<int>("id");
                  if (id <= 0)
                  {
                      context.Errors.Add(new ExecutionError("valid id >= 0 is required"));
                      return null;
                  }

                  var data = await productRepository.GetByIdAsync(id);

                  return data;
              }
            );

            FieldAsync<SupplierGraphType>(
              "supplier",

              arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "id" }),

              resolve: async context =>
              {
                  var id = context.GetArgument<int>("id");
                  if (id <= 0)
                  {
                      context.Errors.Add(new ExecutionError("valid id >= 0 is required"));
                      return null;
                  }

                  var data = await supplierRepository.GetByIdAsync(id);

                  return data;
              }
            );

        }
    }

}
