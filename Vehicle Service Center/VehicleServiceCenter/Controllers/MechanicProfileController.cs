using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<MechanicProfileModel> mechanicProfilesWithRelations =
                context.MechanicProfiles
                    .AsNoTracking()
                    .Include(m => m.User)
                    .Include(m => m.Branch)
                    .ToList();

            var mechanicProfiles = mechanicProfilesWithRelations
                .Select(m => new
                {
                    m.MechanicProfileId,
                    m.UserId,
                    m.BranchId,
                    m.Specialization,
                    m.ExperienceYears,
                    m.HireDate,
                    m.IsAvailable,
                    User = m.User == null
                        ? null
                        : new
                        {
                            m.User.UserName,
                            m.User.Email,
                            m.User.PhoneNumber,
                            m.User.IsActive
                        },
                    Branch = m.Branch == null
                        ? null
                        : new
                        {
                            m.Branch.BranchName,
                            m.Branch.Address,
                            m.Branch.IsActive
                        }
                })
                .ToList();

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

        // Get Mechanics by Branch ID
        [HttpGet("GetByBranchId")]
        public IActionResult GetByBranchId(int branchId)
        {
            List<MechanicProfileModel> mechanicProfilesWithUsers =
                context.MechanicProfiles
                    .AsNoTracking()
                    .Include(m => m.User)
                    .Where(m => m.BranchId == branchId)
                    .ToList();

            var mechanicProfiles = mechanicProfilesWithUsers
                .Select(m => new
                {
                    m.MechanicProfileId,
                    m.UserId,
                    m.BranchId,
                    m.Specialization,
                    m.ExperienceYears,
                    m.HireDate,
                    m.IsAvailable,
                    UserName = m.User?.UserName
                })
                .ToList();

            return Ok(mechanicProfiles);
        }

        // Get Mechanic Profiles sorted by experience
        [HttpGet("GetSortedByExperience")]
        public IActionResult GetSortedByExperience(
            bool descending = true)
        {
            IQueryable<MechanicProfileModel> query =
                context.MechanicProfiles
                    .AsNoTracking()
                    .Include(m => m.User)
                    .Include(m => m.Branch);

            query = descending
                ? query.OrderByDescending(m => m.ExperienceYears)
                    .ThenBy(m => m.MechanicProfileId)
                : query.OrderBy(m => m.ExperienceYears)
                    .ThenBy(m => m.MechanicProfileId);

            List<MechanicProfileModel> sortedMechanics =
                query.ToList();

            var result = sortedMechanics.Select(m => new
                {
                    m.MechanicProfileId,
                    m.UserId,
                    m.BranchId,
                    m.Specialization,
                    m.ExperienceYears,
                    m.HireDate,
                    m.IsAvailable,
                    UserName = m.User?.UserName,
                    BranchName = m.Branch?.BranchName
                })
                .ToList();

            return Ok(result);
        }

        // Update Mechanic Profile
        [HttpPut("UpdateMechanicProfile")]
        public IActionResult UpdateMechanicProfile(
            int id,
            MechanicProfileModel newMechanicProfile)
        {
            MechanicProfileModel? mechanicProfile =
                context.MechanicProfiles.FirstOrDefault(
                    m => m.MechanicProfileId == id
                );

            if (mechanicProfile == null)
            {
                return NotFound("Mechanic profile not found");
            }

            BranchModel? branch = context.Branches.FirstOrDefault(
                b => b.BranchId == newMechanicProfile.BranchId
            );

            if (branch == null)
            {
                return NotFound("Branch not found");
            }

            mechanicProfile.Specialization =
                newMechanicProfile.Specialization;

            mechanicProfile.ExperienceYears =
                newMechanicProfile.ExperienceYears;

            mechanicProfile.HireDate =
                newMechanicProfile.HireDate;

            mechanicProfile.IsAvailable =
                newMechanicProfile.IsAvailable;

            mechanicProfile.BranchId =
                newMechanicProfile.BranchId;

            context.SaveChanges();

            return Ok("Mechanic profile updated successfully");
        }

        // Update Mechanic Availability
        [HttpPatch("UpdateAvailability")]
        public IActionResult UpdateAvailability(
            int id,
            bool isAvailable)
        {
            MechanicProfileModel? mechanicProfile =
                context.MechanicProfiles.FirstOrDefault(
                    m => m.MechanicProfileId == id
                );

            if (mechanicProfile == null)
            {
                return NotFound("Mechanic profile not found");
            }

            mechanicProfile.IsAvailable = isAvailable;
            context.SaveChanges();

            return Ok("Mechanic availability updated successfully");
        }

        // Delete Mechanic Profile
        [HttpDelete("RemoveMechanicProfile")]
        public IActionResult RemoveMechanicProfile(int id)
        {
            MechanicProfileModel? mechanicProfile =
                context.MechanicProfiles.FirstOrDefault(
                    m => m.MechanicProfileId == id
                );

            if (mechanicProfile == null)
            {
                return NotFound("Mechanic profile not found");
            }

            context.MechanicProfiles.Remove(mechanicProfile);
            context.SaveChanges();

            return Ok("Mechanic profile removed successfully");
        }
    }
}
