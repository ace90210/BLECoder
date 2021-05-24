using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLECoder.Blazor.Client.Authentication.Security
{
    /// <summary>
    /// Maps array strogns in claims to seperate claims each, credit to cradle77 (see https://medium.com/@marcodesanctis2/securing-blazor-webassembly-with-identity-server-4-ee44aa1687ef)
    /// </summary>
    /// <typeparam name="TAccount"></typeparam>
    public class ArrayClaimsPrincipalFactory<TAccount> : AccountClaimsPrincipalFactory<TAccount> where TAccount : RemoteUserAccount
    {
        public ArrayClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor)
        : base(accessor)
        { }


        // when a user belongs to multiple roles, IS4 returns a single claim with a serialised array of values
        // this class improves the original factory by deserializing the claims in the correct way
        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(TAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);

            var claimsIdentity = (ClaimsIdentity)user.Identity;

            if (account != null)
            {
                foreach (var additionalProperty in account.AdditionalProperties)
                {
                    var name = additionalProperty.Key;
                    var value = additionalProperty.Value;
                    if (value != null &&
                        value is JsonElement element && element.ValueKind == JsonValueKind.Array)
                    {
                        claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(additionalProperty.Key));

                        var claims = element.EnumerateArray().Select(x => new Claim(additionalProperty.Key, x.ToString()));

                        claimsIdentity.AddClaims(claims);
                    }
                }
            }

            return user;
        }
    }
}
