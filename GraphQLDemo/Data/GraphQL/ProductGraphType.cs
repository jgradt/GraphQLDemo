using GraphQL.Types;
using GraphQLDemo.Data.Entities;
using GraphQLDemo.Data.Repositories;

namespace GraphQLDemo.Data.GraphQL
{
    public class ProductGraphType : ObjectGraphType<Product>
    {
        public ProductGraphType(IOrderDetailRepository orderDetailRepository, ISupplierRepository supplierRepository)
        {
            Name = "Product";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.ProductName);
            Field(x => x.UnitPrice);
            Field(x => x.Discontinued);

            FieldAsync<ListGraphType<NonNullGraphType<OrderDetailsGraphType>>>(
              "orderDetails",

              resolve: async context =>
              {
                  var numItems = context.GetArgument<int>("limit");
                  numItems = numItems > 0 ? numItems : 10;

                  var data = await orderDetailRepository.GetPagedAsync(0, numItems, filter: o => o.ProductId == context.Source.Id);

                  return data.Items;
              }
            );

            FieldAsync<NonNullGraphType<SupplierGraphType>>(
              "supplier",

              resolve: async context =>
              {
                  var data = await supplierRepository.GetByIdAsync(context.Source.SupplierId);

                  return data;
              }
            );

        }
    }

}
