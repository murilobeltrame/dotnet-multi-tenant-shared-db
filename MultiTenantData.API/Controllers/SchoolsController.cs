using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiTenantData.API.Models;

namespace MultiTenantData.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SchoolsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentsController> _logger;

        public SchoolsController(ILogger<StudentsController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<School>>> List()
        {
            return Ok(await _context.Schools.ToListAsync());
        }
    }
}
