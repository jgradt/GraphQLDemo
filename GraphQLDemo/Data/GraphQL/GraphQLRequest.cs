/*
 * See also: https://graphql-dotnet.github.io/docs/getting-started/queries
 * See also: https://developer.okta.com/blog/2019/04/16/graphql-api-with-aspnetcore
 */

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
