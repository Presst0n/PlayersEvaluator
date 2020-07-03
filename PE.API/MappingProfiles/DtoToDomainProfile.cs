using AutoMapper;
using PE.API.Dtos;
using PE.DomainModels;


namespace PE.API.MappingProfiles
{
    public class DtoToDomainProfile : Profile
    {
        public DtoToDomainProfile()
        {
            CreateMap<RosterDto, Roster>();
            CreateMap<RaiderDto, Raider>();
            CreateMap<RefreshTokenDto, RefreshToken>();
            CreateMap<RaiderNoteDto, RaiderNote>();
            CreateMap<UserRosterAccessDto, UserRosterAccess>();
        }
    }
}
