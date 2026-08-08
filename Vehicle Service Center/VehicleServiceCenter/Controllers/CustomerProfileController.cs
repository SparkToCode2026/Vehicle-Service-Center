using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            this.context = context;
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
            List<CustomerProfileModel> customerProfilesWithUsers =
                context.CustomerProfiles
                    .AsNoTracking()
                    .Include(c => c.User)
                    .ToList();

            var customerProfiles = customerProfilesWithUsers
                .Select(c => new
                {
                    c.CustomerProfileId,
                    c.UserId,
                    c.Address,
                    c.DateOfBirth,
                    c.CreatedAt,
                    User = c.User == null
                        ? null
                        : new
                        {
                            c.User.UserName,
                            c.User.Email,
                            c.User.PhoneNumber,
                            c.User.IsActive
                        }
                })
                .ToList();

            return Ok(customerProfiles);
        }

        // Filter Customer Profiles
        [HttpGet("Filter")]
        public IActionResult FilterCustomerProfiles(
            string? address,
            DateOnly? bornFrom,
            DateOnly? bornTo)
        {
            if (string.IsNullOrWhiteSpace(address) &&
                !bornFrom.HasValue &&
                !bornTo.HasValue)
            {
                return BadRequest(
                    "Provide an address or a date-of-birth range"
                );
            }

            if (bornFrom.HasValue &&
                bornTo.HasValue &&
                bornFrom.Value > bornTo.Value)
            {
                return BadRequest(
                    "The start date cannot be after the end date"
                );
            }

            IQueryable<CustomerProfileModel> query =
                context.CustomerProfiles
                    .AsNoTracking()
                    .Include(c => c.User);

            if (!string.IsNullOrWhiteSpace(address))
            {
                query = query.Where(c =>
                    c.Address != null && c.Address.Contains(address));
            }

            if (bornFrom.HasValue)
            {
                query = query.Where(c =>
                    c.DateOfBirth >= bornFrom.Value);
            }

            if (bornTo.HasValue)
            {
                query = query.Where(c =>
                    c.DateOfBirth <= bornTo.Value);
            }

            List<CustomerProfileModel> filteredProfiles =
                query.ToList();

            var result = filteredProfiles.Select(c => new
                {
                    c.CustomerProfileId,
                    c.UserId,
                    c.Address,
                    c.DateOfBirth,
                    c.CreatedAt,
                    UserName = c.User?.UserName
                })
                .ToList();

            return Ok(result);
        }

        // Get Customer Profiles sorted by creation date
        [HttpGet("GetSortedByCreatedAt")]
        public IActionResult GetSortedByCreatedAt(
            bool descending = true)
        {
            IQueryable<CustomerProfileModel> query =
                context.CustomerProfiles
                    .AsNoTracking()
                    .Include(c => c.User);

            query = descending
                ? query.OrderByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.CreatedAt);

            List<CustomerProfileModel> sortedProfiles = query.ToList();

            var result = sortedProfiles.Select(c => new
                {
                    c.CustomerProfileId,
                    c.UserId,
                    c.Address,
                    c.DateOfBirth,
                    c.CreatedAt,
                    UserName = c.User?.UserName
                })
                .ToList();

            return Ok(result);
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

        // Update Customer Address
        [HttpPatch("UpdateAddress")]
        public IActionResult UpdateAddress(
            int id,
            string newAddress)
        {
            CustomerProfileModel? customerProfile =
                context.CustomerProfiles.FirstOrDefault(
                    c => c.CustomerProfileId == id
                );

            if (customerProfile == null)
            {
                return NotFound("Customer profile not found");
            }

            if (string.IsNullOrWhiteSpace(newAddress))
            {
                return BadRequest("Address cannot be empty");
            }

            customerProfile.Address = newAddress;
            context.SaveChanges();

            return Ok("Customer address updated successfully");
        }

        // Delete Customer Profile
        [HttpDelete("RemoveCustomerProfile")]
        public IActionResult RemoveCustomerProfile(int id)
        {
            CustomerProfileModel? customerProfile =
                context.CustomerProfiles.FirstOrDefault(
                    c => c.CustomerProfileId == id
                );

            if (customerProfile == null)
            {
                return NotFound("Customer profile not found");
            }

            context.CustomerProfiles.Remove(customerProfile);
            context.SaveChanges();

            return Ok("Customer profile removed successfully");
        }

    }
}
