using System.Security.Claims;

namespace EMS.Middlewares.AuthorizationMiddleware
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IRoleService roleService)
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                // Get user role from the database
                var role = await roleService.GetRoleByUserIdAsync(int.Parse(userId));

                // Add role to claims
                if (role != null)
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Role, role.ToString()) };
                    var identity = new ClaimsIdentity(claims);
                    context.User.AddIdentity(identity);
                }
            }

            await _next(context);
        }
    }

}
