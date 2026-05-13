namespace CORE.APP.Models.Authentication
{
    /// <summary>
    /// Base class for token requests containing authentication credentials and token generation parameters.
    /// This class serves as the foundation for JWT token generation requests.
    /// </summary>
    public class TokenRequestBase
    {
        /// <summary>
        /// Gets or sets the username for authentication.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for authentication.
        /// </summary>
        public string Password { get; set; }

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
