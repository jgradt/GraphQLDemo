using GraphQL;
using GraphQL.Types;

namespace GraphQLDemo.Data.GraphQL
{
    public class GraphQLQuerySchema : Schema
    {
        public GraphQLQuerySchema(IDependencyResolver resolver)
          : base(resolver)
        {
            Query = resolver.Resolve<GraphQLQuery>();
            //Mutation = resolver.Resolve<GraphQLMutation>();
        }
    }
}
