using GraphQLDemo.Data.Entities;

namespace GraphQLDemo.Data.Repositories
{
    public interface IProductRepository : IBaseDataRepository<Product>
    {
    }

    public class ProductRepository : BaseDataRepository<Product>, IProductRepository
    {

        public ProductRepository(DemoDbContext demoDbContext) : base(demoDbContext)
        {
        }

        public override void SetDataForUpdate(Product sourceEntity, Product destinationEntity)
        {
            destinationEntity.ProductName = sourceEntity.ProductName;
            destinationEntity.UnitPrice = sourceEntity.UnitPrice;
            destinationEntity.Discontinued = sourceEntity.Discontinued;
        }

    }
}
