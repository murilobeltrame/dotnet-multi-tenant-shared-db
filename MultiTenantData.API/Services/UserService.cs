using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MultiTenantData.API.Models;

namespace MultiTenantData.API.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(
            IConfiguration configuration,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserResponse> Create(UserRequest user)
        {
            if (user.Password != user.ConfirmPassword) throw new Exception("Password must match");

            var userModel = _mapper.Map<User>(user);
            userModel.PasswordHash = HashText(user.Password);

            var managedSchools = await _context
                .Schools
                .IgnoreQueryFilters()
                .Where(w => user.ManagedSchools.Contains(w.Id))
                .ToListAsync();
            userModel.ManagedSchools = managedSchools;

            var userEntry = await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResponse>(userEntry.Entity);
        }

        public async Task<int> Delete(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> List()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> Get(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(w => w.Id == userId);
        }

        public async Task<User> Get(string username, string password)
        {
            var hashedPassword = HashText(password);

            return await _context.Users
                .Include(i => i.ManagedSchools)
                .IgnoreQueryFilters()
                .Where(w => w.UserName == username && w.PasswordHash == hashedPassword)
                .FirstOrDefaultAsync();
        }

        private string HashText(string text)
        {
            var sha256 = SHA256.Create();

            var salt = _configuration["Security:PwdSalt"];
            var saltedPassword = $"{salt}.{text}";

            var textBytes = Encoding.ASCII.GetBytes(saltedPassword);
            var hashedBytes = sha256.ComputeHash(textBytes);
            return Encoding.ASCII.GetString(hashedBytes);
        }
    }

    public class UserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IEnumerable<Guid> ManagedSchools { get; set; }
    }

    public class UserResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public IEnumerable<SchoolResponse> ManagedSchools { get; set; }
    }

    public class SchoolResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<School, SchoolResponse>();
            CreateMap<User, UserResponse>()
                .ForMember(dest=> dest.ManagedSchools, opt=> opt.MapFrom(src => src.ManagedSchools));
            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.ManagedSchools, opt => opt.Ignore());
        }
    }
}
