using System;
using System.Collections.Generic;

namespace MultiTenantData.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<School> ManagedSchools { get; set; }
    }
}
