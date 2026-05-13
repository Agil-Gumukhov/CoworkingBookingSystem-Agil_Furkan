namespace CORE.APP.Models.Authentication
{
    /// <summary>
    /// Represents the response containing JWT and refresh token information.
    /// Returned after successful authentication or token refresh.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Gets or sets the JWT (JSON Web Token) that serves as the access token.
        /// This token is used to authorize API requests and expires after a short period (e.g., 5 minutes).
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the refresh token used to obtain a new JWT without requiring credentials.
        /// This token is long-lived (e.g., 7 days) and should be stored securely on the client.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration time (Unix timestamp) of the JWT token.
        /// Indicates when the JWT will no longer be valid for authorization.
        /// </summary>
        public long Expiration { get; set; }
    }
}
