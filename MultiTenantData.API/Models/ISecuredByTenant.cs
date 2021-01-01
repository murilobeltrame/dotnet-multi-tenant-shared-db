using System;

namespace MultiTenantData.API.Models
{
    public class ISecuredByTenant
    {
        Guid? SecuredByTenantId { get; set; }
    }
}