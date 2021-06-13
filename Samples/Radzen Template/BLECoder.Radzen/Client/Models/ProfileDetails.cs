using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace RadzenTemplate.Client.Models
{
    public class ProfileDetails
    {
        public static readonly string[] RecognisedRoles = new[] { "Member", "ExampleRole", "Counter", "WeatherManager" };

        public string Username { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string AuthenticationType { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<Claim> AdditionalInformation { get; set; }

        public ProfileDetails(ClaimsPrincipal user)
        {
            Name = user.Identity?.Name;
            AuthenticationType = user.Identity?.AuthenticationType;

            var claimsList = user.Claims.ToList();
            Username = GetSingleClaim(claimsList, "preferred_username");
            Email = GetSingleClaim(claimsList, "email");
            Roles = GetRoles(claimsList, "role");

            AdditionalInformation = GetSpecifiedClaims(claimsList, "role", "name", "email", "preferred_username");
        }

        private string GetSingleClaim(List<Claim> claims, params string[] type)
        {
            if (claims == null || type == null)
                return null;

            var claimFound = claims.FirstOrDefault(c => type.Any(t => t.Equals(c.Type, StringComparison.InvariantCultureIgnoreCase)));

            //if(claimFound != null)
            //    claims.Remove(claimFound);

            return claimFound?.Value;
        }

        private IEnumerable<string> GetRoles(List<Claim> claims, string type)
        {
            if (claims == null || type == null)
                return null;

            var claimsFound = claims.Where(c => type.Equals(c.Type, StringComparison.InvariantCultureIgnoreCase) && RecognisedRoles.Any(rr => rr.Equals(c.Value, StringComparison.InvariantCultureIgnoreCase)));

            //if(claimsFound.Count() > 0)
            //{
            //    claims.RemoveAll(c => type.Equals(c.Type, StringComparison.InvariantCultureIgnoreCase) && RoleConstants.RecognisedRoles.Any(rr => rr.Equals(c.Value, StringComparison.InvariantCultureIgnoreCase)));
            //}

            return claimsFound.Select(c => c.Value);
        }

        private IEnumerable<Claim> GetSpecifiedClaims(List<Claim> claims, params string[] doNotInclude)
        {
            return claims.Where(c => !doNotInclude.Any(dni => dni.Equals(c.Type, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
