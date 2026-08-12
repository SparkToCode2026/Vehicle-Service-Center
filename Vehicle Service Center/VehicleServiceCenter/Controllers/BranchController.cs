using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Controllers;
[Authorize]
[ApiController]
[Route("Branch")]
public class BranchController : ControllerBase
{
    private ProjectContext context;

    public BranchController(ProjectContext projectContext)
    {
        context = projectContext;
    }

    // Add branch
    [Authorize(Roles = "Admin")]
    [HttpPost("AddBranch")]
    public IActionResult AddBranch(BranchModel branch)
    {
        // Check whether branch name already exists
        BranchModel? existingBranch =
            context.Branches.FirstOrDefault(b =>
                b.BranchName == branch.BranchName
            );

        if (existingBranch != null)
        {
            return BadRequest(
                "Branch name already exists"
            );
        }

        // Check whether branch email already exists
        BranchModel? existingEmail =
            context.Branches.FirstOrDefault(b =>
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

        context.Branches.Add(branch);
        context.SaveChanges();

        return Ok(new
        {
            Message = "Branch added successfully",
            BranchId = branch.BranchId
        });
    }
    
    
    
    // Get all branches
    [AllowAnonymous]
    [HttpGet("GetAll")]
    public IActionResult GetAllBranches()
    {
        var branches = context.Branches
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
    
    // Get with Mechanics 
    [Authorize(Roles = "Admin,Mechanic")]
    [HttpGet("GetAllWithMechanics")]
    public IActionResult GetAllBranchesWithMechanics()
    {
        var branches = context.Branches
            .Include(b => b.MechanicProfiles)
            .Select(b => new
            {
                b.BranchId,
                b.BranchName,
                b.Address,
                b.IsActive,
                MechanicCount = b.MechanicProfiles.Count
            })
            .ToList();

        return Ok(branches);
    }
    
    // Sort By name
    [AllowAnonymous]
    [HttpGet("SortByName")]
    public IActionResult SortBranchesByName(bool descending = false)
    {
        IQueryable<BranchModel> query = context.Branches;

        query = descending
            ? query.OrderByDescending(b => b.BranchName)
            : query.OrderBy(b => b.BranchName);

        var branches = query
            .Select(b => new
            {
                b.BranchId,
                b.BranchName,
                b.IsActive
            })
            .ToList();

        return Ok(branches);
    }
    
    // Count By status 
    [Authorize(Roles = "Admin")]
    [HttpGet("CountByStatus")]
    public IActionResult GetBranchCountByStatus()
    {
        var summary = context.Branches
            .GroupBy(b => b.IsActive)
            .Select(g => new
            {
                IsActive = g.Key,
                Count = g.Count()
            })
            .ToList();

        return Ok(summary);
    }
    
    // Get all active branches
    [AllowAnonymous]
    [HttpGet("GetActive")]
    public IActionResult GetActiveBranches()
    {
        var branches = context.Branches
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
    [AllowAnonymous]
    [HttpGet("GetById/{id}")]
    public IActionResult GetBranchById(int id)
    {
        var branch = context.Branches
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
        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public IActionResult UpdateBranch(
            int id,
            BranchModel updatedBranch
        )
        {
            BranchModel? branch =
                context.Branches.Find(id);

            if (branch == null)
            {
                return NotFound("Branch not found");
            }

            BranchModel? existingBranchName =
                context.Branches.FirstOrDefault(b =>
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

            BranchModel? existingEmail =
                context.Branches.FirstOrDefault(b =>
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

        context.SaveChanges();

            return Ok(new
            {
                Message = "Branch updated successfully",
                BranchId = branch.BranchId
            });
        }
        
        // Change branch status
        [Authorize(Roles = "Admin")]
        [HttpPatch("ChangeStatus/{id}")]
        public IActionResult ChangeBranchStatus(
            int id,
            bool isActive
        )
        {
            BranchModel? branch =
                context.Branches.Find(id);

            if (branch == null)
            {
                return NotFound("Branch not found");
            }

            branch.IsActive = isActive;
            context.SaveChanges();

            return Ok(new
            {
                Message = "Branch status changed successfully",
                BranchId = branch.BranchId,
                IsActive = branch.IsActive
            });
        }
        // Delete branch by ID
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteBranch(int id)
        {
            BranchModel? branch =
                context.Branches.Find(id);

            if (branch == null)
            {
                return NotFound("Branch not found");
            }

            bool hasMechanics =
                context.MechanicProfiles
                    .Any(m => m.BranchId == id);

            bool hasAppointments =
                context.Appointments
                    .Any(a => a.BranchId == id);

            bool hasServiceOrders =
                context.ServiceOrders
                    .Any(s => s.BranchId == id);

            bool hasSpareParts =
                context.SpareParts
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

            context.Branches.Remove(branch);
            context.SaveChanges();

            return Ok(new
            {
                Message = "Branch deleted successfully",
                BranchId = branch.BranchId
            });
        }
}
