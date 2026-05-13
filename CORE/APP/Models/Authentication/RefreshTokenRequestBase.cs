namespace CORE.APP.Models.Authentication
{
    /// <summary>
    /// Base class for refresh token requests containing token refresh parameters.
    /// This class is used to request a new JWT using an existing refresh token.
    /// </summary>
    public class RefreshTokenRequestBase
    {
        /// <summary>
        /// Gets or sets the expired JWT token that needs to be refreshed.
        /// The claims from this token are extracted to identify the user.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the refresh token previously issued with the JWT.
        /// This token is long-lived and is used to obtain a new JWT without requiring credentials.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the security key used to sign and validate JWT tokens.
        /// This is a symmetric key typically stored in configuration and not exposed to the client.
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// Gets or sets the issuer of the JWT token.
        /// Identifies the principal that issued the JWT.
        /// Retrieved from appsettings.json configuration.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the intended audience of the JWT token.
        /// Specifies which applications or services should accept this token.
        /// Retrieved from appsettings.json configuration.
        /// </summary>
        public string Audience { get; set; }
    }
}
