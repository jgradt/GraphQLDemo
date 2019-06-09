/*
 * See also: https://graphql-dotnet.github.io/docs/getting-started/queries
 * See also: https://developer.okta.com/blog/2019/04/16/graphql-api-with-aspnetcore
 * See also: https://medium.com/volosoft/building-graphql-apis-with-asp-net-core-419b32a5305b
 */

using Newtonsoft.Json.Linq;

namespace GraphQLDemo.Data.GraphQL
{
    public class GraphQLRequest
    {
        public string OperationName { get; set; }
        //public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
