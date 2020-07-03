using AutoMapper;
using PE.API.Dtos;
using PE.DomainModels;

namespace PE.API.MappingProfiles
{
    public class DomainToDtoProfile : Profile
    {
        public DomainToDtoProfile()
        {
            CreateMap<Roster, RosterDto>();
            CreateMap<Raider, RaiderDto>();
            CreateMap<RefreshToken, RefreshTokenDto>();
            CreateMap<RaiderNote, RaiderNoteDto>();
            CreateMap<UserRosterAccess, UserRosterAccessDto>();
        }
    }
}
