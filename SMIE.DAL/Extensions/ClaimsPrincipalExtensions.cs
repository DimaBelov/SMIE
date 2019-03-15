using System.Linq;
using System.Security.Claims;

namespace SMIE.DAL.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        const string EMAIL_CLAIM_TYPE = "email";

        public static string UserName(this ClaimsPrincipal principal) => principal.Claims
            .FirstOrDefault(c => c.Type.Equals(ClaimsIdentity.DefaultNameClaimType))?.Value;

        public static string Email(this ClaimsPrincipal principal) => principal.Claims
            .FirstOrDefault(c => c.Type.Equals(EMAIL_CLAIM_TYPE))?.Value;
    }
}
