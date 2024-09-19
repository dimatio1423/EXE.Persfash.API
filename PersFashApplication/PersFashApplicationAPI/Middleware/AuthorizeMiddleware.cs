using BusinessObject.Enums;
using Repositories.FashionInfluencerRepos;
using Repositories.PartnerRepos;
using Repositories.UserRepos;
using Services.Helper.CustomExceptions;
using System.Net;
using System.Security.Claims;

namespace PersFashApplicationAPI.Middleware
{
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICustomerRepository customerRepository, IPartnerRepository partnerRepository, IFashionInfluencerRepository fashionInfluencerRepository)
        {
            try
            {
                var requestPath = context.Request.Path;

                if (requestPath.StartsWithSegments("/api/authentication"))
                {
                    await _next.Invoke(context);
                    return;
                }

                var userIdentity = context.User.Identity as ClaimsIdentity;
                if (!userIdentity.IsAuthenticated)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                var customer = await customerRepository.Get((int.Parse(userIdentity.FindFirst("userid").Value)));
                //var partner = await partnerRepository.Get((int.Parse(userIdentity.FindFirst("userid").Value)));
                var influencer = await fashionInfluencerRepository.Get((int.Parse(userIdentity.FindFirst("userid").Value)));

                if (customer != null)
                {
                    if (customer.Status.Equals(StatusEnums.Inactive.ToString()))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return;
                    }
                }else if (influencer != null)
                {
                    if (influencer.Status.Equals(StatusEnums.Inactive.ToString()))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return;
                    }
                }else
                {
                    throw new ApiException(HttpStatusCode.NotFound, "User not found");
                }

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(ex.ToString());
            }

        }
    }
}
