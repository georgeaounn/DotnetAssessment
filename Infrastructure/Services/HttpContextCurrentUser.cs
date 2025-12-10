
using Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services
{

    public class HttpContextCurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public HttpContextCurrentUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid UserId
        {
            get
            {
                var userIdClaim = _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                  ?? _accessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                return Guid.TryParse(userIdClaim, out var id) ? id : Guid.Empty;
            }
        }

        public int RoleId
        {
            get
            {
                var roleIdClaim = _accessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value
                                 ?? _accessor.HttpContext?.User?.FindFirst("RoleId")?.Value;
                return int.TryParse(roleIdClaim, out var id) ? id : 0;
            }
        }
    }
}