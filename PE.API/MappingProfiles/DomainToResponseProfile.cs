using AutoMapper;
using PE.API.ExtendedModels;
using PE.Contracts.V1.Responses;
using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PE.API.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Roster, RosterResponse>()
                .ForMember(dest => dest.Raiders, memberOptions =>
                    memberOptions.MapFrom(src => src.Raiders.Select(x => new RaiderResponse
                    {
                        RosterId = x.RosterId,
                        Notes = x.Notes.Select(n => new RaiderNoteResponse { RaiderNoteId = n.RaiderNoteId, RaiderId = n.RaiderId, CreatorId = n.CreatorId, Message = n.Message, CreatorName = n.CreatorName }),
                        Class = x.Class,
                        MainSpecialization = x.MainSpecialization,
                        Name = x.Name,
                        OffSpecialization = x.OffSpecialization,
                        Points = x.Points,
                        Role = x.Role,
                        RaiderId = x.RaiderId
                    })));
            CreateMap<Raider, RaiderResponse>()
                .ForMember(dest => dest.Notes, memberOptions =>
                    memberOptions.MapFrom(src => src.Notes.Select(x => new RaiderNoteResponse { RaiderNoteId = x.RaiderNoteId, RaiderId = x.RaiderId, Message = x.Message, CreatorId = x.CreatorId })));
            CreateMap<UserRosterAccess, RosterAccessResponse>();
            CreateMap<RaiderNote, RaiderNoteResponse>();
            CreateMap<ExtendedIdentityUser, UserResponse>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName)); 
        }
    }
}
