using AutoMapper;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Players,
                    opt => opt.MapFrom(src => src.Players.ToList().Count));
        }
    }
}
