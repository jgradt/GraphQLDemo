using GraphQL.Types;
using GraphQLDemo.Data.Entities;
using GraphQLDemo.Data.Repositories;

namespace GraphQLDemo.Data.GraphQL
{
    public class SupplierGraphType : ObjectGraphType<Supplier>
    {
        public SupplierGraphType(IProductRepository productRepository)
        {
            Name = "Supplier";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.CompanyName);
            Field(x => x.ContactName);
            Field(x => x.ContactTitle);
            Field(x => x.StreetAddress);
            Field(x => x.City);
            Field(x => x.State);
            Field(x => x.PostalCode);
            Field(x => x.Phone);

            FieldAsync<ListGraphType<NonNullGraphType<ProductGraphType>>>(
              "products",

              resolve: async context =>
              {
                  var numItems = context.GetArgument<int>("count");
                  numItems = numItems > 0 ? numItems : 10;

                  var data = await productRepository.GetPagedAsync(0, numItems, filter: o => o.SupplierId == context.Source.Id);

                  return data.Items;
              }
            );

        }
    }

}
