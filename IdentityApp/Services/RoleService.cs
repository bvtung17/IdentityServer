using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        public RoleService(RoleManager<IdentityRole<string>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<string> Create(string roleName)
        {
            var role = new IdentityRole { Name = roleName, NormalizedName = roleName };
            var rs = await _roleManager.CreateAsync(role);
            if (rs.Succeeded)
            {
                return "Tao Role Thanh cong";
            }
            return "Tao Role Không Thanh cong";
        }

        public async Task<string> Delete(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return "Không tồn tại role";
            }
            var rs = await _roleManager.DeleteAsync(role);
            if (rs.Succeeded)
            {
                return "Xoa Role thành công";
            }
            return "Xóa Role Thất Bại";
        }

        public async Task<List<IdentityRole<string>>> GetAll()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}
