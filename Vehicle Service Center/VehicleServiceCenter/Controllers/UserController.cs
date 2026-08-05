using Microsoft.AspNetCore.Mvc;

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

        
    }
}
