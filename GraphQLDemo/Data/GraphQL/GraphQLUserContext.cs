using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphQLDemo.Data.GraphQL
{
    public class GraphQLUserContext
    {
        ClaimsPrincipal user;

        public GraphQLUserContext(ClaimsPrincipal user)
        {
            this.user = user;
        }
    }
}
