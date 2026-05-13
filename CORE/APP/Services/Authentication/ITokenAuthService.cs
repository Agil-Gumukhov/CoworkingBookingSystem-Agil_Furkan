using CORE.APP.Models.Authentication;
using System.Security.Claims;

namespace CORE.APP.Services.Authentication
{
    /// <summary>
    /// Interface for token authentication service operations.
    /// Defines contracts for JWT generation, validation, and refresh token operations.
    /// </summary>
    public interface ITokenAuthService
    {
        /// <summary>
        /// Generates a new JWT token for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="userName">The username of the user.</param>
        /// <param name="roles">Array of role names assigned to the user.</param>
        /// <param name="expiration">The expiration date and time of the token.</param>
        /// <param name="securityKey">The security key used to sign the token.</param>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The intended audience of the token.</param>
        /// <param name="refreshToken">The refresh token to be included in the response.</param>
        /// <returns>A TokenResponse object containing the JWT and refresh token.</returns>
        TokenResponse GetTokenResponse(int userId, string userName, string[] roles, DateTime expiration, 
            string securityKey, string issuer, string audience, string refreshToken);

        /// <summary>
        /// Generates a cryptographically secure refresh token.
        /// </summary>
        /// <returns>A randomly generated refresh token string.</returns>
        string GetRefreshToken();

        /// <summary>
        /// Extracts and validates claims from an expired JWT token.
        /// </summary>
        /// <param name="token">The JWT token (may be expired).</param>
        /// <param name="securityKey">The security key used to validate the token signature.</param>
        /// <returns>A collection of claims from the token if valid; otherwise, an empty collection.</returns>
        IEnumerable<Claim> GetClaims(string token, string securityKey);
    }
}
