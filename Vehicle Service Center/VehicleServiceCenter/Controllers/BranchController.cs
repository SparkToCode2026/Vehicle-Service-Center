using Microsoft.AspNetCore.Mvc;

namespace VehicleServiceCenter.Controllers;

[ApiController]
[Route("Branch")]
public class BranchController : ControllerBase
{
    private ProjectContext ProjectContext;

    public BranchController(ProjectContext projectContext)
    {
        ProjectContext = projectContext;
    }

    // Add branch
    [HttpPost("AddBranch")]
    public IActionResult AddBranch(BranchModel branch)
    {
        // Check whether branch name already exists
        BranchModel existingBranch =
            ProjectContext.Branches.FirstOrDefault(b =>
                b.BranchName == branch.BranchName
            );

        if (existingBranch != null)
        {
            return BadRequest(
                "Branch name already exists"
            );
        }

        // Check whether branch email already exists
        BranchModel existingEmail =
            ProjectContext.Branches.FirstOrDefault(b =>
                b.Email == branch.Email
            );

        if (existingEmail != null)
        {
            return BadRequest(
                "Branch email already exists"
            );
        }

        if (branch.OpeningTime >= branch.ClosingTime)
        {
            return BadRequest(
                "Opening time must be before closing time"
            );
        }

        branch.IsActive = true;

        ProjectContext.Branches.Add(branch);
        ProjectContext.SaveChanges();

        return Ok(new
        {
            Message = "Branch added successfully",
            BranchId = branch.BranchId
        });
    }
    
    // Get all branches
    [HttpGet("GetAll")]
    public IActionResult GetAllBranches()
    {
        var branches = ProjectContext.Branches
            .Select(b => new
            {
                b.BranchId,
                b.BranchName,
                b.Address,
                b.PhoneNumber,
                b.Email,
                b.OpeningTime,
                b.ClosingTime,
                b.IsActive
            })
            .ToList();

        return Ok(branches);
    }
    
    // Get all active branches
    [HttpGet("GetActive")]
    public IActionResult GetActiveBranches()
    {
        var branches = ProjectContext.Branches
            .Where(b => b.IsActive == true)
            .Select(b => new
            {
                b.BranchId,
                b.BranchName,
                b.Address,
                b.PhoneNumber,
                b.Email,
                b.OpeningTime,
                b.ClosingTime,
                b.IsActive
            })
            .ToList();

        return Ok(branches);
    }
    
    // Get branch by ID
    [HttpGet("GetById/{id}")]
    public IActionResult GetBranchById(int id)
    {
        var branch = ProjectContext.Branches
            .Where(b => b.BranchId == id)
            .Select(b => new
            {
                b.BranchId,
                b.BranchName,
                b.Address,
                b.PhoneNumber,
                b.Email,
                b.OpeningTime,
                b.ClosingTime,
                b.IsActive
            })
            .FirstOrDefault();

        if (branch == null)
        {
            return NotFound("Branch not found");
        }

        return Ok(branch);
    }
    
    // Update branch by ID
        [HttpPut("Update/{id}")]
        public IActionResult UpdateBranch(
            int id,
            BranchModel updatedBranch
        )
        {
            BranchModel branch =
                ProjectContext.Branches.Find(id);

            if (branch == null)
            {
                return NotFound("Branch not found");
            }

            BranchModel existingBranchName =
                ProjectContext.Branches.FirstOrDefault(b =>
                    b.BranchName ==
                        updatedBranch.BranchName &&
                    b.BranchId != id
                );

            if (existingBranchName != null)
            {
                return BadRequest(
                    "Branch name already exists"
                );
            }

            BranchModel existingEmail =
                ProjectContext.Branches.FirstOrDefault(b =>
                    b.Email == updatedBranch.Email &&
                    b.BranchId != id
                );

            if (existingEmail != null)
            {
                return BadRequest(
                    "Branch email already exists"
                );
            }

            if (updatedBranch.OpeningTime >=
                updatedBranch.ClosingTime)
            {
                return BadRequest(
                    "Opening time must be before closing time"
                );
            }

            branch.BranchName =
                updatedBranch.BranchName;
            branch.Address =
                updatedBranch.Address;
            branch.PhoneNumber =
                updatedBranch.PhoneNumber;
            branch.Email =
                updatedBranch.Email;
            branch.OpeningTime =
                updatedBranch.OpeningTime;
            branch.ClosingTime =
                updatedBranch.ClosingTime;
            branch.IsActive =
                updatedBranch.IsActive;

            ProjectContext.SaveChanges();

            return Ok(new
            {
                Message = "Branch updated successfully",
                BranchId = branch.BranchId
            });
        }
        
        // Change branch status
        [HttpPatch("ChangeStatus/{id}")]
        public IActionResult ChangeBranchStatus(
            int id,
            bool isActive
        )
        {
            BranchModel branch =
                ProjectContext.Branches.Find(id);

            if (branch == null)
            {
                return NotFound("Branch not found");
            }

            branch.IsActive = isActive;
            ProjectContext.SaveChanges();

            return Ok(new
            {
                Message = "Branch status changed successfully",
                BranchId = branch.BranchId,
                IsActive = branch.IsActive
            });
        }
        // Delete branch by ID
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteBranch(int id)
        {
            BranchModel branch =
                ProjectContext.Branches.Find(id);

            if (branch == null)
            {
                return NotFound("Branch not found");
            }

            bool hasMechanics =
                ProjectContext.MechanicProfiles
                    .Any(m => m.BranchId == id);

            bool hasAppointments =
                ProjectContext.Appointments
                    .Any(a => a.BranchId == id);

            bool hasServiceOrders =
                ProjectContext.ServiceOrders
                    .Any(s => s.BranchId == id);

            bool hasSpareParts =
                ProjectContext.SpareParts
                    .Any(s => s.BranchId == id);

            if (hasMechanics ||
                hasAppointments ||
                hasServiceOrders ||
                hasSpareParts)
            {
                return BadRequest(
                    "Branch cannot be deleted because it has related records. Change its status to inactive instead"
                );
            }

            ProjectContext.Branches.Remove(branch);
            ProjectContext.SaveChanges();

            return Ok(new
            {
                Message = "Branch deleted successfully",
                BranchId = branch.BranchId
            });
        }
}