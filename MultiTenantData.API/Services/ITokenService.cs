using MultiTenantData.API.Models;

namespace MultiTenantData.API.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}