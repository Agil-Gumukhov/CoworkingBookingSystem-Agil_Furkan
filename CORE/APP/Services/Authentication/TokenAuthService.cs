using CORE.APP.Models.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CORE.APP.Services.Authentication
{
    /// <summary>
    /// Concrete implementation of token authentication service for JWT operations.
    /// Provides methods for generating JWT tokens, refresh tokens, and extracting claims from tokens.
    /// </summary>
    public class TokenAuthService : ITokenAuthService
    {
        /// <summary>
        /// Generates a TokenResponse containing a JWT token and refresh token for a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="userName">The username of the user.</param>
        /// <param name="roles">Array of role names assigned to the user.</param>
        /// <param name="expiration">The expiration date and time of the JWT token.</param>
        /// <param name="securityKey">The security key used to sign the token (minimum 256 bits).</param>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The intended audience of the token.</param>
        /// <param name="refreshToken">The refresh token to be included in the response.</param>
        /// <returns>A TokenResponse containing the JWT and refresh token with expiration time.</returns>
        public TokenResponse GetTokenResponse(int userId, string userName, string[] roles, DateTime expiration,
            string securityKey, string issuer, string audience, string refreshToken)
        {
            // Create a symmetric security key from the provided security key string
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            // Create signing credentials using the security key and HS256 algorithm
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Initialize claims collection with user information
            var claims = new List<Claim>
            {
                new Claim("Id", userId.ToString()), // User ID for identifying the user
                new Claim(ClaimTypes.Name, userName) // Username for user identification
            };

            // Add role claims for each role assigned to the user
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create the JWT descriptor with all required information
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };

            // Create a JWT token handler to generate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Convert the token to a string representation
            var tokenString = tokenHandler.WriteToken(token);

            // Convert expiration to Unix timestamp (seconds since epoch)
            var expirationTimestamp = new DateTimeOffset(expiration).ToUnixTimeSeconds();

            // Return the token response containing JWT and refresh token
            return new TokenResponse
            {
                Token = tokenString,
                RefreshToken = refreshToken,
                Expiration = expirationTimestamp
            };
        }

        /// <summary>
        /// Generates a cryptographically secure refresh token.
        /// </summary>
        /// <returns>A base64-encoded refresh token string.</returns>
        public string GetRefreshToken()
        {
            // Create a byte array of 32 bytes for high security
            var randomNumber = new byte[32];

            // Use cryptographically secure random number generator
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            // Convert the random bytes to base64 string for safe transmission
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Extracts claims from a JWT token without validating expiration.
        /// This is useful for extracting user information from expired tokens (e.g., to refresh them).
        /// </summary>
        /// <param name="token">The JWT token string.</param>
        /// <param name="securityKey">The security key used to validate the token signature.</param>
        /// <returns>A collection of claims from the token if valid; otherwise, an empty collection.</returns>
        public IEnumerable<Claim> GetClaims(string token, string securityKey)
        {
            try
            {
                // Create JWT token handler for validation and claim extraction
                var tokenHandler = new JwtSecurityTokenHandler();

                // Create symmetric security key from the security key string
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

                // Configure token validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidateIssuerSigningKey = true,

                    // Do not validate expiration - we want to extract claims from expired tokens
                    ValidateLifetime = false,

                    // Disable other validations for claim extraction from expired tokens
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                // Validate and extract the token
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                // Return the claims from the validated token
                return principal.Claims;
            }
            catch
            {
                // Return empty collection if token validation fails
                return Enumerable.Empty<Claim>();
            }
        }
    }
}
