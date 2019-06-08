using GraphQLDemo.Data.Entities;

namespace GraphQLDemo.Data.Repositories
{
    public interface ISupplierRepository : IBaseDataRepository<Supplier>
    {
    }

    public class SupplierRepository : BaseDataRepository<Supplier>, ISupplierRepository
    {

        public SupplierRepository(DemoDbContext demoDbContext) : base(demoDbContext)
        {
        }

        public override void SetDataForUpdate(Supplier sourceEntity, Supplier destinationEntity)
        {
            destinationEntity.CompanyName = sourceEntity.CompanyName;
            destinationEntity.ContactName = sourceEntity.ContactName;
            destinationEntity.ContactTitle = sourceEntity.ContactTitle;
            destinationEntity.StreetAddress = sourceEntity.StreetAddress;
            destinationEntity.City = sourceEntity.City;
            destinationEntity.State = sourceEntity.State;
            destinationEntity.PostalCode = sourceEntity.PostalCode;
            destinationEntity.Phone = sourceEntity.Phone;
        }

    }
}
