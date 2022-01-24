using IdentityApp.Models;
using IdentityApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(string name, string password)
        {
            return Ok(await _userService.Login(name, password));
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(string userName, string password)
        {
            return Ok(await _userService.Register( userName, password));
        }

    }
}
