using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Services
{
    public interface IRoleService
    {
        Task<List<IdentityRole<string>>> GetAll();
        Task<string> Create(string roleName);
        Task<string> Delete(string roleName);
    }
}
