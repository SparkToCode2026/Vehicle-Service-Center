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
}