using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PE.API.Extensions;
using PE.API.Helpers;
using PE.API.Services;
using PE.Contracts.V1;
using PE.Contracts.V1.Requests;
using PE.Contracts.V1.Responses;
using PE.DomainModels;
using PE.DomainModels.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PE.API.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RosterAccessController : Controller
    {
        private readonly IRosterAccessService _rosterAccessService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public RosterAccessController(IRosterAccessService rosterAccessService, IMapper mapper, IUriService uriService)
        {
            _rosterAccessService = rosterAccessService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(ApiRoutes.RosterAccesses.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllRosterAccessesRequest request, [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var loggedUserId = HttpContext.GetUserId();
            var callerAccess = await _rosterAccessService.GetRosterAccessAsync(GetBy.RosterId, loggedUserId, request.RosterId);

            if (callerAccess is null)
                return CreateErrorResponse(Status.BadRequest);

            if (!callerAccess.IsOwner)
            {
                if (!callerAccess.IsModerator)
                    return CreateErrorResponse(Status.BadRequest);
            }

            var accessModels = await _rosterAccessService.GetRosterAccessesByRosterIdAsync(request.RosterId, pagination);

            var rostersAccessResponse = _mapper.Map<List<RosterAccessResponse>>(accessModels);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<RosterAccessResponse>(rostersAccessResponse));
            }

            var paginationResponse = PaginationHelper.CreatePaginatedResponse(_uriService, pagination, rostersAccessResponse);

            return Ok(paginationResponse);
        }

        [HttpPost(ApiRoutes.RosterAccesses.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRosterAccessRequest request)
        {
            var loggedUserId = HttpContext.GetUserId();

            var access = await _rosterAccessService.GetRosterAccessAsync(GetBy.RosterId, loggedUserId, request.RosterId);

            if (access is null)
                return CreateErrorResponse(Status.Forbidden);

            if (access.UserId == request.UserId)
                return CreateErrorResponse(Status.BadRequest, "You already have access to this roster.");

            var newAccess = new UserRosterAccess
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                RosterId = request.RosterId,
                CreatorId = loggedUserId,
                IsModerator = request.IsModerator,
                CreatedOn = DateTime.UtcNow
            };

            await _rosterAccessService.CreateRosterAccessAsync(newAccess);
            var locationUri = _uriService.GetRosterAccessUri(newAccess.Id.ToString());

            return Created(locationUri, _mapper.Map<RosterAccessResponse>(newAccess));
        }

        [HttpPut(ApiRoutes.RosterAccesses.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid rosterAccessId, [FromBody] UpdateRosterAccessRequest request)
        {
            var loggedUserId = HttpContext.GetUserId();

            var rosterAccessToUpdate = await _rosterAccessService.GetRosterAccessByIdAsync(rosterAccessId);

            if (rosterAccessToUpdate is null)
                return NotFound();

            var callersRosterAccess = await _rosterAccessService.GetRosterAccessAsync(GetBy.RosterId, loggedUserId, rosterAccessToUpdate.RosterId);

            if (callersRosterAccess is null)
                return CreateErrorResponse(Status.Forbidden);

            if (!callersRosterAccess.IsOwner)
            {
                if (!callersRosterAccess.IsModerator)
                    return CreateErrorResponse(Status.Forbidden);
            }

            if (callersRosterAccess.IsModerator && !callersRosterAccess.IsOwner && rosterAccessToUpdate.IsOwner)
            {
                return CreateErrorResponse(Status.Forbidden);
            }

            if (callersRosterAccess.IsModerator && !callersRosterAccess.IsOwner && rosterAccessToUpdate.IsModerator)
            {
                if (!request.IsModerator)
                {
                    return CreateErrorResponse(Status.Forbidden);
                }
                else
                {
                    return CreateErrorResponse(Status.BadRequest, "User is already moderator.");
                }
            }

            rosterAccessToUpdate.IsModerator = request.IsModerator;
            await _rosterAccessService.UpdateRosterAccessAsync(rosterAccessToUpdate);

            return Ok(_mapper.Map<RosterAccessResponse>(rosterAccessToUpdate));
        }

        [HttpDelete(ApiRoutes.RosterAccesses.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid rosterAccessId)
        {
            var loggedUserId = HttpContext.GetUserId();

            var rosterAccessToDelete = await _rosterAccessService.GetRosterAccessByIdAsync(rosterAccessId);

            if (rosterAccessToDelete is null)
                return NotFound();

            var callersRosterAccess = await _rosterAccessService.GetRosterAccessAsync(GetBy.RosterId, loggedUserId, rosterAccessToDelete.RosterId);

            if (callersRosterAccess is null)
                return CreateErrorResponse(Status.Forbidden);

            if (!callersRosterAccess.IsOwner)
            {
                return CreateErrorResponse(Status.Forbidden);
            }

            if (callersRosterAccess.CreatorId == rosterAccessToDelete.UserId)
            {
                return CreateErrorResponse(Status.Forbidden, "You cannot delete your own roster access.");
            }

            await _rosterAccessService.DeleteRosterAccessAsync(rosterAccessToDelete);

            return NoContent();
        }

        private IActionResult CreateErrorResponse(Status httpStatusCode, string message = null, string title = null)
        {
            return httpStatusCode switch
            {
                Status.BadRequest => StatusCode((int)httpStatusCode, new ErrorResponse(new ErrorModel
                {
                    Title = !string.IsNullOrEmpty(title) ? title : "BadRequest",
                    Message = !string.IsNullOrEmpty(message) ? message : "Client-side input fails validation."
                })),
                Status.Unauthorized => StatusCode((int)httpStatusCode, new ErrorResponse(new ErrorModel
                {
                    Title = !string.IsNullOrEmpty(title) ? title : "Unauthorized",
                    Message = !string.IsNullOrEmpty(message) ? message : "Insufficient permissions"
                })),
                Status.Forbidden => StatusCode((int)httpStatusCode, new ErrorResponse(new ErrorModel
                {
                    Title = !string.IsNullOrEmpty(title) ? title : "Forbidden",
                    Message = !string.IsNullOrEmpty(message) ? message : "Insufficient permissions"
                })),
                _ => null,
            };
        }
    }
}
