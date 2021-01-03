using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MultiTenantData.API.Services;

namespace MultiTenantData.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<StudentsController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create([FromBody] UserRequest userRequest)
        {
            var user = await _userService.Create(userRequest);
            return Created("", user);
        }
    }
}
