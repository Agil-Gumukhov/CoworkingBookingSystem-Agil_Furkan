using CORE.APP.Models.Authentication;
using CORE.APP.Services.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Users.APP.Domain;

namespace Users.APP.Features.Tokens
{
    public class RefreshTokenRequest : RefreshTokenRequestBase, IRequest<TokenResponse>
    {
    }

    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, TokenResponse>
    {
        private readonly DbContext _db;
        private readonly ITokenAuthService _tokenAuthService;

        public RefreshTokenHandler(DbContext db, ITokenAuthService tokenAuthService)
        {
            _db = db;
            _tokenAuthService = tokenAuthService;
        }

        public async Task<TokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var claims = _tokenAuthService.GetClaims(request.Token, request.SecurityKey);
            var userIdClaim = claims.SingleOrDefault(claim => claim.Type == "Id");
            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out var userId))
                return null;

            var user = await _db.Set<User>()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Id == userId && u.RefreshToken == request.RefreshToken
                    && u.RefreshTokenExpiration >= DateTime.Now && u.IsActive, cancellationToken);

            if (user is null)
                return null;

            user.RefreshToken = _tokenAuthService.GetRefreshToken();
            user.RefreshTokenExpiration = DateTime.Now.AddDays(7);

            _db.Set<User>().Update(user);
            await _db.SaveChangesAsync(cancellationToken);

            var roles = user.UserRoles.Select(ur => ur.Role.Name).Distinct().ToArray();
            if (!roles.Any())
                roles = ["User"];

            var expiration = DateTime.Now.AddMinutes(5);

            return _tokenAuthService.GetTokenResponse(user.Id, user.UserName, roles,
                expiration, request.SecurityKey, request.Issuer, request.Audience, user.RefreshToken);
        }
    }
}
