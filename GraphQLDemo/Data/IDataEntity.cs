using System;

namespace GraphQLDemo.Data
{
    public interface IDataEntity
    {
        int Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime LastUpdatedDate { get; set; }
    }
}
