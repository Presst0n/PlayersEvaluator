﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public class RaiderNoteController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IRaiderNoteService _raiderNoteService;
        private readonly IRosterAccessService _rosterAccessService;
        private readonly IRaiderService _raiderService;

        public RaiderNoteController(IMapper mapper, IUriService uriService, IRaiderNoteService raiderNoteService,
            IRosterAccessService rosterAccessService, IRaiderService raiderService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _raiderNoteService = raiderNoteService;
            _rosterAccessService = rosterAccessService;
            _raiderService = raiderService;
        }

        [HttpGet(ApiRoutes.RaiderNotes.Get)]
        public async Task<IActionResult> Get([FromRoute] string raiderNoteId)
        {
            var loggedInUserId = HttpContext.GetUserId();

            var raiderNote = await _raiderNoteService.GetRaiderNoteByIdAsync(raiderNoteId);

            if (raiderNote is null)
                return NotFound();

            var rosterAccess = await _rosterAccessService.GetRosterAccessAsync(GetBy.RaiderId, loggedInUserId, raiderNote.RaiderId);

            if (rosterAccess is null)
                return Forbid();

            return Ok(new Response<RaiderNoteResponse>(_mapper.Map<RaiderNoteResponse>(raiderNote)));
        }

        [HttpGet(ApiRoutes.RaiderNotes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] string raiderId, [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            //var raider = await _raiderService.GetRaiderByIdAsync(raiderId);

            //if (raider is null)
            //    return BadRequest("RaiderId is incorrect or raider with given id does not exists.");

            var rosterAccess = await _rosterAccessService.GetRosterAccessAsync(GetBy.RaiderId, HttpContext.GetUserId(), raiderId);

            if (rosterAccess is null)
            {
                return NotFound();
            }

            var raiderNotes = await _raiderNoteService.GetRaiderNotesByRaiderIdAsync(raiderId, pagination);

            var raiderNoteResponse = _mapper.Map<List<RaiderNoteResponse>>(raiderNotes);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<RaiderNoteResponse>(raiderNoteResponse));
            }

            var paginationResponse = PaginationHelper.CreatePaginatedResponse(_uriService, pagination, raiderNoteResponse);

            return Ok(paginationResponse);
        }

        [HttpPost(ApiRoutes.RaiderNotes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRaiderNoteRequest raiderNoteRequest)
        {
            //var raider = await _raiderService.GetRaiderByIdAsync(raiderNoteRequest.RaiderId);

            //if (raider is null)
            //    return BadRequest("Cannot create note for raider, because it does not exists.");

            var rosterAccess = await _rosterAccessService.GetRosterAccessAsync(GetBy.RaiderId, HttpContext.GetUserId(), raiderNoteRequest.RaiderId);

            if (rosterAccess is null)
            {
                return Forbid();
            }

            if (!rosterAccess.IsOwner)
            {
                if (!rosterAccess.IsModerator)
                {
                    return Forbid();
                }
            }

            var raiderNoteId = Guid.NewGuid().ToString();

            var raiderNote = new RaiderNote
            {
                RaiderId = raiderNoteRequest.RaiderId,
                RaiderNoteId = raiderNoteId,
                Message = raiderNoteRequest.Message,
                CreatorId = rosterAccess.UserId
            };

            await _raiderNoteService.CreateRaiderNoteAsync(raiderNote);

            var uri = _uriService.GetRaiderNoteUri(raiderNote.RaiderNoteId);

            return Created(uri, _mapper.Map<RaiderNoteResponse>(raiderNote));
        }

        [HttpPut(ApiRoutes.RaiderNotes.Update)]
        public async Task<IActionResult> Update([FromRoute] string raiderNoteId, [FromBody] UpdateRaiderNoteRequest request)
        {
            var loggedInUser = HttpContext.GetUserId();

            var raiderNote = await _raiderNoteService.GetRaiderNoteByIdAsync(raiderNoteId);

            if (raiderNote is null)
                return NotFound();

            var rosterAccess = await _rosterAccessService.GetRosterAccessAsync(GetBy.RaiderId, loggedInUser, raiderNote.RaiderId);

            if (!rosterAccess.IsOwner)
            {
                if (!rosterAccess.IsModerator)
                {
                    return Forbid();
                }
            }

            raiderNote.Message = request.Message;

            await _raiderNoteService.UpdateRaiderNoteAsync(raiderNote);

            return Ok(_mapper.Map<RaiderNoteResponse>(raiderNote));
        }

        [HttpDelete(ApiRoutes.RaiderNotes.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string raiderNoteId)
        {
            var loggedInUser = HttpContext.GetUserId();

            var raiderNote = await _raiderNoteService.GetRaiderNoteByIdAsync(raiderNoteId);

            if (raiderNote is null)
                return NotFound();

            var rosterAccess = await _rosterAccessService.GetRosterAccessAsync(GetBy.RaiderId, loggedInUser, raiderNote.RaiderId);

            if (rosterAccess is null)
                return Forbid();

            if (!rosterAccess.IsOwner)
            {
                if (!rosterAccess.IsModerator)
                {
                    return Forbid();
                } 
            }

            await _raiderNoteService.DeleteRaiderNoteAsync(raiderNote);

            return NoContent();
        }
    }
}
