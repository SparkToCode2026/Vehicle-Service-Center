using Microsoft.AspNetCore.Mvc;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [ApiController]
    [Route("MechanicProfile")]
    public class MechanicProfileController : ControllerBase
    {
        private ProjectContext context;

        public MechanicProfileController(ProjectContext projectContext)
        {
            context = projectContext;
        }

        // Add Mechanic Profile
        [HttpPost("AddMechanicProfile")]
        public IActionResult AddMechanicProfile(
            MechanicProfileModel mechanicProfile)
        {
            // Check if the user exists
            UserModel? user = context.Users.FirstOrDefault(
                u => u.UserId == mechanicProfile.UserId
            );

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Only users with the Mechanic role can have a mechanic profile
            if (!user.Role.Equals(
                "Mechanic",
                StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(
                    "The selected user does not have the Mechanic role"
                );
            }

            // Check whether this user already has a mechanic profile
            MechanicProfileModel? existingProfile =
                context.MechanicProfiles.FirstOrDefault(
                    m => m.UserId == mechanicProfile.UserId
                );

            if (existingProfile != null)
            {
                return BadRequest(
                    "This user already has a mechanic profile"
                );
            }

            // Check whether the branch exists
            BranchModel? branch = context.Branches.FirstOrDefault(
                b => b.BranchId == mechanicProfile.BranchId
            );

            if (branch == null)
            {
                return NotFound("Branch not found");
            }

            mechanicProfile.MechanicProfileId = 0;
            mechanicProfile.IsAvailable = true;

            context.MechanicProfiles.Add(mechanicProfile);
            context.SaveChanges();

            return Ok(new
            {
                Message = "Mechanic profile added successfully",
                mechanicProfile.MechanicProfileId
            });
        }

        // Get Mechanic Profile by ID
        [HttpGet("GetMechanicProfile")]
        public IActionResult GetMechanicProfile(int id)
        {
            MechanicProfileModel? mechanicProfile =
                context.MechanicProfiles.FirstOrDefault(
                    m => m.MechanicProfileId == id
                );

            if (mechanicProfile == null)
            {
                return NotFound("Mechanic profile not found");
            }

            return Ok(mechanicProfile);
        }

        // Get All Mechanic Profiles
        [HttpGet("GetAllMechanicProfiles")]
        public IActionResult GetAllMechanicProfiles()
        {
            List<MechanicProfileModel> mechanicProfiles =
                context.MechanicProfiles.ToList();

            return Ok(mechanicProfiles);
        }

        // Get Mechanic Profile by User ID
        [HttpGet("GetByUserId")]
        public IActionResult GetByUserId(int userId)
        {
            MechanicProfileModel? mechanicProfile =
                context.MechanicProfiles.FirstOrDefault(
                    m => m.UserId == userId
                );

            if (mechanicProfile == null)
            {
                return NotFound("Mechanic profile not found");
            }

            return Ok(mechanicProfile);
        }

    }
}
