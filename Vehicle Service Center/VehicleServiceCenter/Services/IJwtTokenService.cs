using VehicleServiceCenter.DTOs;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Services
{
    public interface IJwtTokenService
    {
        LoginResponse CreateToken(UserModel user);
    }
}
