using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Auth.API.Presentation.Attributes
{
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                var jwt = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var key = Encoding.ASCII.GetBytes(jwt["Jwt:Key"]!);

                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                context.HttpContext.User = claimsPrincipal;
            }
            catch
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
