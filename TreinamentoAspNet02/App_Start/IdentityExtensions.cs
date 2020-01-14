using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Security.Principal;

namespace TreinamentoAspNet02.App_Start
{
    public static class IdentityExtensions
    {
        public static string GetName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Nome");
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}