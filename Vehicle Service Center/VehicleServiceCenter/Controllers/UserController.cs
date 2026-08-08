using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private ProjectContext context;

        public UserController(ProjectContext context)
        {
            this.context = context;
        }

        // Register User
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(UserModel user)
        {
            // Check whether the email already exists
            UserModel existingUser = context.Users
                .FirstOrDefault(u => u.Email == user.Email);

            if (existingUser != null)
            {
                return BadRequest("Email is already registered");
            }

            // Check if the role is valid
            if (user.Role != "Customer" &&
                user.Role != "Mechanic" &&
                user.Role != "Admin")
            {
                return BadRequest(
                    "Role must be Customer, Mechanic, or Admin"
                );
            }

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
        [HttpPost("Login")]

        // Login User
        public IActionResult Login(string email, string password)
        {
            // Find the user by email
            UserModel user = context.Users
                .FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }
            // Verify the password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password");
            }
            // Check if the user is active
            if (!user.IsActive)
            {
                return Unauthorized("User account is inactive");
            }
            return Ok(new
            {
                Message = "Login successful",
                UserId = user.UserId,
                Role = user.Role
            });
        }

        // Get all users
        [HttpGet("GetAll")]
        public IActionResult GetAllUsers()
        {
            var users = context.Users.Select(u => new
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
        [HttpPut("Update/{id}")]
        public IActionResult UpdateUser(int id, UserModel updatedUser)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
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

        // Delete user by ID
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
