using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PE.API.Extensions;
using PE.API.Helpers;
using PE.API.Services;
using PE.Contracts.V1;
using PE.Contracts.V1.Requests;
using PE.Contracts.V1.Responses;
using PE.DomainModels;
using PE.DomainModels.Enums;

namespace PE.API.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RaiderController : Controller
    {
        private readonly IRaiderService _raiderSerivce;
        private readonly IRosterService _rosterService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;

        public RaiderController(IRaiderService raiderSerivce, IUriService uriService, IMapper mapper,
            IResourceAuthorizationService resourceAuthorizationService, IRosterService rosterService)
        {
            _raiderSerivce = raiderSerivce;
            _uriService = uriService;
            _mapper = mapper;
            _resourceAuthorizationService = resourceAuthorizationService;
            _rosterService = rosterService;
        }

        [HttpGet(ApiRoutes.Raiders.Get)]
        public async Task<IActionResult> Get([FromRoute] string raiderId)
        {
            var raider = await _raiderSerivce.GetRaiderByIdAsync(raiderId);

            if (raider is null)
                return NotFound();

            var result = await _resourceAuthorizationService.AuthorizeAsync(HttpContext.GetUserId(), raider);

            if (!result.ReadOnlyAccess)
                return CreateErrorResponse(Status.Forbidden);

            return Ok(new Response<RaiderResponse>(_mapper.Map<RaiderResponse>(raider)));
        }

        [HttpGet(ApiRoutes.Raiders.GetAll)]
        public async Task<IActionResult> GetAllFrom([FromQuery] string rosterId, [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var result = await _resourceAuthorizationService.AuthorizeAsync(HttpContext.GetUserId(), rosterId);

            if (!result.ReadOnlyAccess)
                return CreateErrorResponse(Status.Forbidden);

            var raiders = await _raiderSerivce.GetRaidersFromRosterAsync(rosterId, pagination);

            if (raiders is null)
                return NotFound();

            var raiderResponse = _mapper.Map<List<RaiderResponse>>(raiders);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<RaiderResponse>(raiderResponse));
            }

            var paginationResponse = PaginationHelper.CreatePaginatedResponse(_uriService, pagination, raiderResponse);

            return Ok(paginationResponse);
        }

        [HttpPost(ApiRoutes.Raiders.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRaiderRequest raiderRequest)
        {
            var roster = await _rosterService.GetRosterByIdAsync(raiderRequest.RosterId);

            if (roster is null)
                return BadRequest("Cannot add raider while given roster does not exist.");

            var result = await _resourceAuthorizationService.AuthorizeAsync(HttpContext.GetUserId(), roster);

            if (!result.IsOwner)
            {
                if (!result.IsModerator)
                {
                    return CreateErrorResponse(Status.Forbidden);
                }
            }

            var raiderId = Guid.NewGuid().ToString();

            var raider = new Raider
            {
                RosterId = raiderRequest.RosterId,
                RaiderId = raiderId,
                Name = raiderRequest.Name,
                Class = raiderRequest.Class,
                Role = raiderRequest.Role,
                MainSpecialization = raiderRequest.MainSpecialization,
                OffSpecialization = raiderRequest.OffSpecialization
            };

            await _raiderSerivce.CreateRaiderAsync(raider);

            var uri = _uriService.GetRaiderUri(raiderId);

            return Created(uri, _mapper.Map<RaiderResponse>(raider));
        }

        [HttpPut(ApiRoutes.Raiders.Update)]
        public async Task<IActionResult> Update([FromRoute] string raiderId, [FromBody] UpdateRaiderRequest request)
        {
            var raider = await _raiderSerivce.GetRaiderByIdAsync(raiderId);

            if (raider is null)
                return NotFound();

            var result = await _resourceAuthorizationService.AuthorizeAsync(HttpContext.GetUserId(), raider);

            if (!result.IsOwner)
            {
                if (!result.IsModerator)
                {
                    return CreateErrorResponse(Status.Forbidden);
                }
            }

            raider.MainSpecialization = request.MainSpecialization;
            raider.OffSpecialization = request.OffSpecialization;
            raider.Points = request.Points;
            raider.Name = request.Name;
            raider.Role = request.Role;
            raider.Class = request.Class;

            await _raiderSerivce.UpdateRaiderAsync(raider);

            return Ok(new Response<RaiderResponse>(_mapper.Map<RaiderResponse>(raider)));
        }

        [HttpDelete(ApiRoutes.Raiders.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string raiderId)
        {
            var raider = await _raiderSerivce.GetRaiderByIdAsync(raiderId);

            if (raider is null)
                return NotFound();

            var result = await _resourceAuthorizationService.AuthorizeAsync(HttpContext.GetUserId(), raider);

            if (!result.IsOwner)
            {
                if (!result.IsModerator)
                {
                    return CreateErrorResponse(Status.Forbidden);
                }
            }

            await _raiderSerivce.DeleteRaiderAsync(raider);

            return NoContent();
        }

        private IActionResult CreateErrorResponse(Status httpStatusCode, string message = null, string title = null)
        {
            switch (httpStatusCode)
            {
                case Status.BadRequest:
                    return StatusCode((int)httpStatusCode, new ErrorResponse(new ErrorModel
                    {
                        Title = !string.IsNullOrEmpty(title) ? title : "BadRequest",
                        Message = !string.IsNullOrEmpty(message) ? message : "Client-side input fails validation."
                    }));
                case Status.Unauthorized:
                    return StatusCode((int)httpStatusCode, new ErrorResponse(new ErrorModel
                    {
                        Title = !string.IsNullOrEmpty(title) ? title : "Unauthorized",
                        Message = !string.IsNullOrEmpty(message) ? message : "Insufficient permissions"
                    }));
                case Status.Forbidden:
                    return StatusCode((int)httpStatusCode, new ErrorResponse(new ErrorModel
                    {
                        Title = !string.IsNullOrEmpty(title) ? title : "Forbidden",
                        Message = !string.IsNullOrEmpty(message) ? message : "Insufficient permissions"
                    }));

                default:
                    return null;
            }
        }
    }
}
