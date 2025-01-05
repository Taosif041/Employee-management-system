using EMS.Models;
using System.Security.Claims;

namespace EMS.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string username);
        string GenerateRefreshToken();
    }
}
