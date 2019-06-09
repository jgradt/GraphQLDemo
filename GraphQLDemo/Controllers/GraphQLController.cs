using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQLDemo.Data.GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GraphQLDemo.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        private ISchema _schema;
        private ILogger<GraphQLController> _log;

        public GraphQLController(ISchema schema, ILogger<GraphQLController> log)
        {
            _schema = schema;
            _log = log;
        }

        public async Task<IActionResult> Post([FromBody] GraphQLRequest queryRequest)
        {

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = _schema;
                _.Query = queryRequest.Query;
                _.OperationName = queryRequest.OperationName;
                _.UserContext = new GraphQLUserContext(User);
                _.Inputs = queryRequest.Variables.ToInputs();
            });

            if (result.Errors?.Count > 0)
            {
                _log.LogError($"GraphQL Request {JsonConvert.SerializeObject(queryRequest)}");
                _log.LogError($"Error(s) processing GraphQL request: {JsonConvert.SerializeObject(result.Errors.Select(e => e.Message))}");
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}