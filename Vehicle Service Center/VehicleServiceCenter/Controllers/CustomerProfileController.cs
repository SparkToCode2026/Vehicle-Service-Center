using Microsoft.AspNetCore.Mvc;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [ApiController]
    [Route("CustomerProfile")]
    public class CustomerProfileController : ControllerBase
    {
        private ProjectContext context;

        public CustomerProfileController(ProjectContext context)
        {
            context = context;
        }

        // Add Customer Profile
        [HttpPost("AddCustomerProfile")]
        public IActionResult AddCustomerProfile(
            CustomerProfileModel customerProfile)
        {
            // Check whether the user exists
            UserModel? user = context.Users.FirstOrDefault(
                u => u.UserId == customerProfile.UserId
            );

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Only a Customer user can have a customer profile
            if (!user.Role.Equals(
                "Customer",
                StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(
                    "The selected user does not have the Customer role"
                );
            }

            // Check whether the user already has a customer profile
            CustomerProfileModel? existingProfile =
                context.CustomerProfiles.FirstOrDefault(
                    c => c.UserId == customerProfile.UserId
                );

            if (existingProfile != null)
            {
                return BadRequest(
                    "This user already has a customer profile"
                );
            }

            // Check the date of birth
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            if (customerProfile.DateOfBirth > today)
            {
                return BadRequest(
                    "Date of birth cannot be in the future"
                );
            }

            customerProfile.CustomerProfileId = 0;
            customerProfile.CreatedAt = DateTime.Now;

            context.CustomerProfiles.Add(customerProfile);
            context.SaveChanges();

            return Ok(new
            {
                Message = "Customer profile added successfully",
                customerProfile.CustomerProfileId
            });
        }

        // Get Customer Profile by ID
        [HttpGet("GetCustomerProfile")]
        public IActionResult GetCustomerProfile(int id)
        {
            CustomerProfileModel? customerProfile =
                context.CustomerProfiles.FirstOrDefault(
                    c => c.CustomerProfileId == id
                );

            if (customerProfile == null)
            {
                return NotFound("Customer profile not found");
            }

            return Ok(customerProfile);
        }

        // Get Customer Profile by User ID
        [HttpGet("GetByUserId")]
        public IActionResult GetByUserId(int userId)
        {
            CustomerProfileModel? customerProfile =
                context.CustomerProfiles.FirstOrDefault(
                    c => c.UserId == userId
                );

            if (customerProfile == null)
            {
                return NotFound("Customer profile not found");
            }

            return Ok(customerProfile);
        }

        // Get All Customer Profiles
        [HttpGet("GetAllCustomerProfiles")]
        public IActionResult GetAllCustomerProfiles()
        {
            List<CustomerProfileModel> customerProfiles =
                context.CustomerProfiles.ToList();

            return Ok(customerProfiles);
        }

        // Update Customer Profile
        [HttpPut("UpdateCustomerProfile")]
        public IActionResult UpdateCustomerProfile(
            int id,
            CustomerProfileModel newCustomerProfile)
        {
            CustomerProfileModel? customerProfile =
                context.CustomerProfiles.FirstOrDefault(
                    c => c.CustomerProfileId == id
                );

            if (customerProfile == null)
            {
                return NotFound("Customer profile not found");
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            if (newCustomerProfile.DateOfBirth > today)
            {
                return BadRequest(
                    "Date of birth cannot be in the future"
                );
            }

            customerProfile.Address = newCustomerProfile.Address;
            customerProfile.DateOfBirth =
                newCustomerProfile.DateOfBirth;

            context.SaveChanges();

            return Ok("Customer profile updated successfully");
        }

    }
}
