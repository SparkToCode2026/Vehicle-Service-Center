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
    
    // GET All Branches
    [HttpGet]
    public async Task<IActionResult> GetAllBranches()
    {
        var branches = await context.Branches
            .ToListAsync();

        return Ok(branches);
    }
    
}