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
        private ProjectContext ProjectContext;

        public UserController(ProjectContext projectContext)
        {
            ProjectContext = projectContext;
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
    }
}
