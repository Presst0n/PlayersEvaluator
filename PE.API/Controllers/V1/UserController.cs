using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PE.API.ExtendedModels;
using PE.API.Extensions;
using PE.Contracts.V1;
using PE.Contracts.V1.Responses;

namespace PE.API.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly UserManager<ExtendedIdentityUser> _userManager;

        public UserController(UserManager<ExtendedIdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet(ApiRoutes.Users.Get)]
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return NotFound("User doesn't exist");
            }

            return Ok(new UserResponse
            {
                Email = user.Email,
                UserId = user.Id,
                UserName = user.UserName,
                CreatedAt = user.CreatedAt.ToString()
            });
        }
    }
}
