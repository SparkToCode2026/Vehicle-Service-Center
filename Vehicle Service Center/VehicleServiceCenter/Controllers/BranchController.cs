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
    }
}