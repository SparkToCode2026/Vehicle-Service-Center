using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.DTOs;
using VehicleServiceCenter.Models;
using VehicleServiceCenter.Services;

namespace VehicleServiceCenter.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly ProjectContext context;
        private readonly JwtTokenService jwtTokenService;

        public UserController(
            ProjectContext context,
            JwtTokenService jwtTokenService)
        {
            this.context = context;
            this.jwtTokenService = jwtTokenService;
        }

        // Register User
        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(UserModel user)
        {
            // Check whether the email already exists
            UserModel? existingUser = context.Users
                .FirstOrDefault(u => u.Email == user.Email);

            if (existingUser != null)
            {
                return BadRequest("Email is already registered");
            }

            // Public registration must not allow role escalation.
            user.Role = "Customer";

            // Hash the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            user.IsActive = true;
            user.CreatedAt = DateTime.Now;

            context.Users.Add(user);
            context.SaveChanges();

            return Ok(new
            {
                Message = "User registered successfully",
                UserId = user.UserId
            });
        }
        // Login User
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Find the user by email
            UserModel? user = context.Users
                .FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }
            // Verify the password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.Password);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password");
            }
            // Check if the user is active
            if (!user.IsActive)
            {
                return Unauthorized("User account is inactive");
            }
            LoginResponse response = jwtTokenService.CreateToken(user);
            return Ok(response);
        }

        // Get all users
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public IActionResult GetAllUsers()
        {
            var usersWithProfiles = context.Users
                .AsNoTracking()
                .Include(u => u.CustomerProfile)
                .Include(u => u.MechanicProfile)
                .ToList();

            var users = usersWithProfiles.Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.Email,
                    u.Role,
                    u.PhoneNumber,
                    u.IsActive,
                    u.CreatedAt,
                    CustomerProfile = u.CustomerProfile == null
                        ? null
                        : new
                        {
                            u.CustomerProfile.CustomerProfileId,
                            u.CustomerProfile.Address,
                            u.CustomerProfile.DateOfBirth
                        },
                    MechanicProfile = u.MechanicProfile == null
                        ? null
                        : new
                        {
                            u.MechanicProfile.MechanicProfileId,
                            u.MechanicProfile.BranchId,
                            u.MechanicProfile.Specialization,
                            u.MechanicProfile.ExperienceYears,
                            u.MechanicProfile.IsAvailable
                        }
                })
                .ToList();
            return Ok(users);
        }

        // Get user by ID
        [HttpGet("GetById/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = context.Users.Where(u => u.UserId == id).Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.Email,
                    u.Role,
                    u.PhoneNumber,
                    u.IsActive,
                    u.CreatedAt
                })
                .FirstOrDefault();
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        // Update user by ID
        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public IActionResult UpdateUser(int id, UserModel updatedUser)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            string normalizedEmail = updatedUser.Email.Trim();

            bool emailAlreadyExists = context.Users
                .AsNoTracking()
                .Any(u =>
                    u.UserId != id &&
                    u.Email == normalizedEmail);

            if (emailAlreadyExists)
            {
                return BadRequest("Email is already registered");
            }

            user.UserName = updatedUser.UserName;
            user.Email = normalizedEmail;
            user.Role = updatedUser.Role;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.IsActive = updatedUser.IsActive;
            context.SaveChanges();
            return Ok(new
            {
                Message = "User updated successfully",
                UserId = user.UserId
            });
        }

        // Change password for user by ID
        [HttpPut("ChangePassword/{id}")]
        public IActionResult ChangePassword(int id,string currentPassword,string newPassword)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            // Verify the current password
            bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, user.Password);
            if (!isCurrentPasswordValid)
            {
                return Unauthorized("Current password is incorrect");
            }
            // Hash the new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            context.SaveChanges();
            return Ok(new
            {
                Message = "Password changed successfully",
                UserId = user.UserId
            });
        }

        // Change user status by ID
        [Authorize(Roles = "Admin")]
        [HttpPatch("ChangeStatus/{id}")]
        public IActionResult ChangeUserStatus(int id, bool isActive)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            user.IsActive = isActive;
            context.SaveChanges();
            return Ok(new
            {
                Message = "User status changed successfully",
                UserId = user.UserId,
                IsActive = user.IsActive
            });
        }

        // Filter users by role and optional active status
        [Authorize(Roles = "Admin")]
        [HttpGet("FilterByRole")]
        public IActionResult FilterByRole(string role, bool? isActive)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest("Role is required");
            }

            var query = context.Users
                .AsNoTracking()
                .Where(u => u.Role == role);

            if (isActive.HasValue)
            {
                query = query.Where(u => u.IsActive == isActive.Value);
            }

            var users = query
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.Email,
                    u.Role,
                    u.PhoneNumber,
                    u.IsActive,
                    u.CreatedAt
                })
                .ToList();

            return Ok(users);
        }

        // Aggregate users by role
        [Authorize(Roles = "Admin")]
        [HttpGet("GetRoleSummary")]
        public IActionResult GetRoleSummary()
        {
            var roleSummary = context.Users
                .AsNoTracking()
                .GroupBy(u => u.Role)
                .Select(group => new
                {
                    Role = group.Key,
                    TotalUsers = group.Count(),
                    ActiveUsers = group.Count(u => u.IsActive),
                    InactiveUsers = group.Count(u => !u.IsActive)
                })
                .OrderByDescending(group => group.TotalUsers)
                .ThenBy(group => group.Role)
                .ToList();

            return Ok(roleSummary);
        }

        // Delete user by ID
        //[Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            context.Users.Remove(user);
            context.SaveChanges();
            return Ok(new
            {
                Message = "User deleted successfully",
                UserId = user.UserId
            });
        }

    }
}
