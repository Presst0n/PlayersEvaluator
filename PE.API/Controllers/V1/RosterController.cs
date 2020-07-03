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
using PE.LoggerService;

namespace PE.API.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RosterController : Controller
    {
        private readonly IRosterService _rosterService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IRosterAccessService _rosterAccessService;
        private readonly IRaiderService _raiderService;
        private readonly ILoggerManager _logger;

        public RosterController(IRosterService rosterService, IMapper mapper, IUriService uriService, IRosterAccessService rosterAccessService,
            IRaiderService raiderService, ILoggerManager logger)
        {
            _rosterService = rosterService;
            _mapper = mapper;
            _uriService = uriService;
            _rosterAccessService = rosterAccessService;
            _raiderService = raiderService;
            _logger = logger;
        }

        [HttpGet(ApiRoutes.Rosters.Get)]
        public async Task<IActionResult> Get([FromRoute] string rosterId)
        {
            var rosterAccess = _rosterAccessService.GetRosterAccessAsync(GetBy.RosterId, HttpContext.GetUserId(), rosterId);

            if (rosterAccess is null)
                return CreateErrorResponse(Status.Forbidden);

            var roster = await _rosterService.GetRosterByIdAsync(rosterId);

            if (roster == null)
                return NotFound();
            //_logger.LogInfo("Getting all rosters.");
            return Ok(new Response<RosterResponse>(_mapper.Map<RosterResponse>(roster)));
        }

        [HttpGet(ApiRoutes.Rosters.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var loggedUserId = HttpContext.GetUserId();
            var rosterAccessModel = await _rosterAccessService.GetRosterAccessesByUserIdAsync(loggedUserId);
            var rosters = _rosterService.GetRosters(rosterAccessModel, pagination);
            var rostersResponse = _mapper.Map<List<RosterResponse>>(rosters);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<RosterResponse>(rostersResponse));
            }

            var paginationResponse = PaginationHelper.CreatePaginatedResponse(_uriService, pagination, rostersResponse);

            return Ok(paginationResponse);
        }

        [HttpPost(ApiRoutes.Rosters.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRosterRequest rosterRequest)
        {
            var loggedUserId = HttpContext.GetUserId();

            var roster = new Roster
            {
                Id = Guid.NewGuid().ToString(),
                CreatorId = loggedUserId,
                CreatorName = rosterRequest.CreatorName,
                Name = rosterRequest.Name,
                Description = rosterRequest.Description
            };

            var userRosterAccess = new UserRosterAccess
            {
                Id = Guid.NewGuid(),
                CreatorId = loggedUserId,
                IsOwner = true,
                IsModerator = false,
                RosterId = roster.Id,
                UserId = loggedUserId,
                CreatedOn = DateTime.UtcNow
            };

            await _rosterService.CreateRosterAsync(roster);
            await _rosterAccessService.CreateRosterAccessAsync(userRosterAccess);

            var locationUri = _uriService.GetRosterUri(roster.Id.ToString());

            return Created(locationUri, _mapper.Map<RosterResponse>(roster));
        }

        [HttpPut(ApiRoutes.Rosters.Update)]
        public async Task<IActionResult> Update([FromRoute] string rosterId, [FromBody] UpdateRosterRequest request)
        {
            if (request == null)
                return ValidationProblem();

            var rosterAccessModel = await _rosterAccessService.GetRosterAccessAsync(GetBy.RosterId, HttpContext.GetUserId(), rosterId);

            if (rosterAccessModel == null)
                return CreateErrorResponse(Status.Forbidden);

            if (!rosterAccessModel.IsOwner)
            {
                if (!rosterAccessModel.IsModerator)
                {
                    return CreateErrorResponse(Status.Forbidden);
                }
            }

            var roster = await _rosterService.GetRosterByIdAsync(rosterId);

            if (roster is null)
                return NotFound();

            roster.Name = request.Name;
            roster.Description = request.Description;

            await _rosterService.UpdateRosterAsync(roster);

            return Ok(new Response<RosterResponse>(_mapper.Map<RosterResponse>(roster)));
        }

        [HttpDelete(ApiRoutes.Rosters.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string rosterId)
        {
            var rosterAccessModel = await _rosterAccessService.GetRosterAccessAsync(GetBy.RosterId, HttpContext.GetUserId(), rosterId);
            var roster = await _rosterService.GetRosterByIdAsync(rosterId);
            
            if (roster is null)
                return NotFound();

            if (rosterAccessModel is null)
                return CreateErrorResponse(Status.Forbidden);

            if (!rosterAccessModel.IsOwner)
            {
                return CreateErrorResponse(Status.Forbidden);
            }

            await _rosterService.DeleteRosterAsync(roster);
            await _rosterAccessService.DeleteRosterAccessesAsync(rosterAccessModel.RosterId);

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
