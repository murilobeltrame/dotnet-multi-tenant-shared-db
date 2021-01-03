using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MultiTenantData.API.Models;

namespace MultiTenantData.API.Services
{
    public interface IUserService
    {
        Task<UserResponse> Create(UserRequest user);
        Task<int> Delete(Guid userId);
        Task<User> Get(Guid userId);
        Task<User> Get(string username, string password);
        Task<IEnumerable<User>> List();
    }
}