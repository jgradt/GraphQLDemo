using GraphQL.Types;
using GraphQLDemo.Data.Entities;
using GraphQLDemo.Data.Repositories;

namespace GraphQLDemo.Data.GraphQL
{
    public class OrderDetailsGraphType : ObjectGraphType<OrderDetail>
    {
        public OrderDetailsGraphType(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            Name = "OrderDetail";

            //Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.Quantity);
            Field(x => x.UnitPrice);
            Field(x => x.TotalPrice);

            FieldAsync<NonNullGraphType<OrderGraphType>>(
              "order",

              resolve: async context =>
              {
                  var data = await orderRepository.GetByIdAsync(context.Source.OrderId);

                  return data;
              }
            );

            FieldAsync<NonNullGraphType<ProductGraphType>>(
              "product",

              resolve: async context =>
              {
                  var data = await productRepository.GetByIdAsync(context.Source.ProductId);

                  return data;
              }
            );

        }
    }

}
