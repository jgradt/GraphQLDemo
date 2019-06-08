using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQLDemo.Data.GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraphQLDemo.Controllers
{
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        private GraphQLQuery _graphQLQuery;
        private ISchema _schema;

        public GraphQLController(GraphQLQuery graphQLQuery, ISchema schema)
        {
            _graphQLQuery = graphQLQuery;
            _schema = schema;
        }

        public async Task<IActionResult> Post([FromBody] GraphQLRequest queryRequest)
        {
            //var inputs = query.Variables.ToInputs();

            //var schema = new Schema
            //{
            //    Query = _graphQLQuery
            //};

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = _schema;
                _.Query = queryRequest.Query;
                _.OperationName = queryRequest.OperationName;
                _.UserContext = new GraphQLUserContext();
                _.Inputs = queryRequest.Variables.ToInputs();
                _.ExposeExceptions = true;
            });

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}