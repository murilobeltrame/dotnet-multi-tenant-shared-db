using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace MultiTenantData.API.Util.Security
{
    public class ClaimsProvider : IClaimsProvider
    {
        private readonly IHttpContextAccessor _accessor;

        public int UserId => int.TryParse(
            _accessor
            .HttpContext?
            .User?.
            Claims?.
            SingleOrDefault(x => x.Type == "UserId")?
            .Value, out var userid) ? userid : 0;

        public IEnumerable<Guid> AcessibleSchoolIds => _accessor
            .HttpContext?
            .User?
            .Claims?
            .Where(x => x.Type == "AccessibleSchoolId")
            .Select(x => Guid.Parse(x.Value))
            .ToList() ?? new List<Guid>();

        public ClaimsProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}
