using System;
using System.Collections.Generic;

namespace MultiTenantData.API.Util.Security
{
    public interface IClaimsProvider
    {
        int UserId { get; }
        IEnumerable<Guid> AcessibleSchoolIds { get; }
    }
}