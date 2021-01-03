using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MultiTenantData.API.Services;

namespace MultiTenantData.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public TokensController(
            ILogger<StudentsController> logger,
            IMapper mapper,
            ITokenService tokenService,
            IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<LogonResponse>> Create([FromBody] LogonRequest logon)
        {
            var user = await _userService.Get(logon.Username, logon.Password);
            if (user == null) return Unauthorized();
            var token = _tokenService.GenerateToken(user);
            var userResponse = _mapper.Map<UserResponse>(user);
            return Created("", new LogonResponse
            {
                AccessToken = token,
                User = userResponse
            });
        }
    }

    public class LogonRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LogonResponse
    {
        public UserResponse User { get; set; }
        public string AccessToken { get; set; }
    }
}
