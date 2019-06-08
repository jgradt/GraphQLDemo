using GraphQLDemo.Data.Entities;

namespace GraphQLDemo.Data.Repositories
{
    public interface IOrderDetailRepository : IBaseDataRepository<OrderDetail>
    {
    }

    public class OrderDetailRepository : BaseDataRepository<OrderDetail>, IOrderDetailRepository
    {

        public OrderDetailRepository(DemoDbContext demoDbContext) : base(demoDbContext)
        {
        }

        public override void SetDataForUpdate(OrderDetail sourceEntity, OrderDetail destinationEntity)
        {
            destinationEntity.Quantity = sourceEntity.Quantity;
        }

    }
}
