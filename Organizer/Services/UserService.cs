using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Repositories;

namespace Organizer.Services
{
    public interface IUserService
    {
        Task<(string TenantId, string UserId, string Email, string FullName)> GetUserInfo();
        string GetTenantId();
        string GetUserId();
        string GetEmail();
        string GetFullName();

    }

    public class UserService : IUserService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly OrganizerContext _context;
        private readonly ICurrentTenantService _currentTenantService;

        public UserService(IRoleRepository roleRepository, OrganizerContext context, IHttpContextAccessor httpContextAccessor, ICurrentTenantService currentTenantService)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _roleRepository = roleRepository;
            _currentTenantService = currentTenantService;
        }

        public string GetTenantId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        }

        public string GetEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("preferred_username")?.Value;
        }

        public string GetFullName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value;
        }

        public async Task<(string TenantId, string UserId, string Email, string FullName)> GetUserInfo()
        {
            if (!await _roleRepository.RoleExistsAsync("Default"))
            {
                // Create the role with a GUID id
                var roleId = Guid.NewGuid();
                var role = new Role
                {
                    id = roleId,
                    title = "Default",
                    tenant_id = _currentTenantService.tenantid,
                    create_team = true,
                    assign_task = true,
                    usermanagement = true,
                    create_task = true
                };
                await _roleRepository.CreateRoleAsync(role);
            }
                var tenantId = GetTenantId();
                var userId = GetUserId();
                var email = GetEmail();
                var fullName = GetFullName();
                var roles = _context.Roles
                .Where(r => r.title == "Default" && r.tenant_id == tenantId)
                .FirstOrDefault();
                var role_id = roles.id;

            var user = _context.Users.Where(u => u.email == email).FirstOrDefault();
                if (user == null)
                {
                    user = new();
                    user.id = userId;
                    user.tenant_id = tenantId;
                    user.email = email;
                    user.fullname = fullName;
                    user.role_id = role_id;
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    Console.WriteLine("userinfo: " + tenantId + " " + userId + " " + email + " " + fullName);

                }
                return (tenantId, userId, email, fullName);
            }
        }
    }