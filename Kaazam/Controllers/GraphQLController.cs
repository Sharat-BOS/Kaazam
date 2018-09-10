using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kaazam.Models;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kaazam.Controllers
{
         [Route("graphql")]
        public class GraphQLController : Controller
        {
            private readonly IDocumentExecuter _documentExecuter;
            private readonly ISchema _schema;

            public GraphQLController(IDocumentExecuter documentExecuter, ISchema schema)
            {
                _documentExecuter = documentExecuter;
                _schema = schema;
            }

            [HttpPost]
            public async Task<IActionResult> Post([FromBody]GraphQLQuery query)
            {
                if (query == null) { throw new ArgumentNullException(nameof(query)); }

                var executionOptions = new ExecutionOptions { Schema = _schema, Query = query.Query };

                try
                {
                    var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

                    if (result.Errors?.Count > 0)
                    {
                        return BadRequest(result);
                    }

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
        }    
}
