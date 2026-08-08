using VehicleServiceCenter.DTOs;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Services
{
    public class IJwtTokenService
    {
        LoginResponse CreateToken(UserModel user);
    }
}
