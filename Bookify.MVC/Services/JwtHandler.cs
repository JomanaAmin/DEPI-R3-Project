using System.Net.Http.Headers;
using System.Security.Claims;

namespace Bookify.MVC.Services
{
    public class JwtHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public JwtHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Get the JWT from the user's claims
            var token = _accessor.HttpContext?.User?.FindFirst("JWT")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
