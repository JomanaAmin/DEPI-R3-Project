using System.Net.Http.Headers;

namespace Bookify.MVC.Services
{
    public class JwtHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor accessor;

        public JwtHandler(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = accessor.HttpContext.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }

}
