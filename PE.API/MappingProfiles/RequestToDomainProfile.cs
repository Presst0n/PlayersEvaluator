using AutoMapper;
using PE.Contracts.V1.Requests;
using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PE.API.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>()
                .ForMember(dest => dest.PageSize, memberOptions =>
                memberOptions.MapFrom(src => src.PageSize > 100 ? 100 : src.PageSize));
            CreateMap<UpdateRosterRequest, Roster>();
        }
    }
}
