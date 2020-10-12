using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PE.API.ExtendedModels;
using PE.API.Extensions;
using PE.API.Helpers;
using PE.API.Services;
using PE.Contracts.V1;
using PE.Contracts.V1.Requests;
using PE.Contracts.V1.Responses;
using PE.DomainModels;

namespace PE.API.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public UserController(UserManager<ExtendedIdentityUser> userManager, IMapper mapper, IUriService uriService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(ApiRoutes.Users.Get)]
        public async Task<IActionResult> Get([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return NotFound("User with given ID doesn't exist");
            }

            return Ok(new UserResponse
            {
                Email = user.Email,
                UserId = user.Id,
                UserName = user.UserName,
                CreatedAt = user.CreatedAt.ToString()
            });
        }

        [HttpGet(ApiRoutes.Users.GetLoggedIn)]
        public async Task<IActionResult> GetLoggedIn()
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

        [HttpGet(ApiRoutes.Users.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var userId = HttpContext.GetUserId();
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            var users = _mapper.Map<List<UserResponse>>(await _userManager.Users.Skip(skip).Take(pagination.PageSize).ToListAsync());


            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<UserResponse>(users));
            }

            var pagedResponse = PaginationHelper.CreatePaginatedResponse(_uriService, pagination, users);

            return Ok(pagedResponse);
        }
    }
}
