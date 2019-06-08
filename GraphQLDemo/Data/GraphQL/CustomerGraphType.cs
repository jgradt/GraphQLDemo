using AutoMapper;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.Data.Dto;
using WebApiDemo.Data.Repositories;
using WebApiDemo.Models;

namespace GraphQLDemo.Data.GraphQL
{
    public class CustomerGraphType : ObjectGraphType<CustomerDto>
    {
        public CustomerGraphType(IMapper mapper, IOrderRepository orderRepository)
        {
            Name = "Customer";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.FirstName);
            Field(x => x.LastName);
            Field(x => x.Email);

            FieldAsync<ListGraphType<OrderGraphType>>(
              "orders",

              arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "count" }),

              resolve: async context =>
              {
                  var numItems = context.GetArgument<int>("count");
                  numItems = numItems > 0 ? numItems : 10;

                  var data = await orderRepository.GetPagedAsync(0, numItems, filter: o => o.CustomerId == context.Source.Id);
                  var mappedData = mapper.Map<List<OrderDto>>(data.Items);

                  return mappedData;
              }
            );
        }
    }

    public class OrderGraphType : ObjectGraphType<OrderDto>
    {
        public OrderGraphType()
        {
            Name = "Order";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.OrderDate);
            Field(x => x.DeliveredDate, nullable: true);
            //TODO: status enum
            //Field(x => x.Status, type: typeof(OrderStatusEnum));
            Field(x => x.TotalDue);
            Field(x => x.Comment);
        }
    }

    public class OrderStatusEnum : EnumerationGraphType<WebApiDemo.Data.Entities.OrderStatus>
    {
    }

    public class GraphQLQuery : ObjectGraphType
    {
        public GraphQLQuery(IMapper mapper, ICustomerRepository customerRepository, 
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
                  var mappedData = mapper.Map<CustomerDto>(data);

                  return mappedData;
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
                  var mappedData = mapper.Map<OrderDto>(data);

                  return mappedData;
              }
            );

        }
    }

    public class QuerySchema : Schema
    {
        public QuerySchema(IDependencyResolver resolver)
          : base(resolver)
        {
            Query = resolver.Resolve<GraphQLQuery>();
            //Mutation = resolver.Resolve<StarWarsMutation>();
        }
    }

    public class GraphQLUserContext
    {
    }

}
