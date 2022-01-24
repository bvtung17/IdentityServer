using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<string> ChangePassword(string userName, string password, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return "Tài khoản không tồn tại";
            }
            var rs = await _userManager.ChangePasswordAsync(user, password, newPassword);
            if(!rs.Succeeded)
            {
                return "Đổi Pass không được";
            }
            return "Đổi password thành công";
        }

        public async Task<string> EmailConfirm(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user== null)
            {
                return "Tài khoản không tồn tại";
            }
            var toke = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var rs = _userManager.ConfirmEmailAsync(user, toke);
            if (!rs.IsCompleted)
            {
                return "Xác thực email không thành công";
            }
            return "Xác thực email thành công";
        }

        public async Task<User> GetUser(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<string> Login(string userName, string password)
        {
            var rs = await _signInManager.PasswordSignInAsync(userName, password, true, true);
            var user = await _userManager.FindByNameAsync(userName);
            if (!rs.Succeeded || user == null)
            {
                return "đăng nhập hụt";
            }
            return await GenerateTokenAsync(user);
        }

        public Task<string> Logout(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(string userName, string password)
        {
            var user = new User { UserName = userName };
            await _userManager.CreateAsync(user, password);
            return "Done";
        }

        public async Task<string> ResetPass(string userName, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return "Tài khoản không tồn tại";
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var rs = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if(!rs.Succeeded)
            {
                return "Reset Pass không được";
            }
            return "Reset Pass được rồi";
        }

        public async Task<string> RoleAssign(string userName, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var rs = await _userManager.AddToRoleAsync(user, roleName);
            if(rs.Succeeded)
            {
                return "Gán role thành công";
            }
            return "Gán role không thành công";
        }
        private async Task<string> GenerateTokenAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.UserName),
                new Claim(ClaimTypes.Role,string.Join(";",roles)),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
