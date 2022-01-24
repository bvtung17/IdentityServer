using IdentityApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string userName);
        Task<string> Login(string userName, string password);
        Task<string> Register(string userName, string password);
        Task<string> EmailConfirm(string userName);
        Task<string> ChangePassword(string userName, string password, string newPassword);
        Task<string> ResetPass(string userName, string newPassword);
        Task<string> Logout(string userName);
        Task<string> RoleAssign(string userName, string roleName);
    }
}
