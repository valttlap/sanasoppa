using AutoMapper;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Game, GameDto>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Players,
                opt => opt.MapFrom(src => src.Players.ToList().Count));
        CreateMap<Player, PlayerDto>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.IsHost,
                opt => opt.Ignore());
        CreateMap<Explanation, ExplanationDto>()
            .ForMember(dest => dest.PlayerId,
                opt => opt.MapFrom(src => src.Player.Id))
            .ForMember(dest => dest.PlayerName,
                opt => opt.MapFrom(src => src.Player.Username))
            .ForMember(dest => dest.Explanation,
                opt => opt.MapFrom(src => src.Round.Word));
        CreateMap<Explanation, VoteExplanationDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Explanation,
                opt => opt.MapFrom(src => src.Round.Word));
        CreateMap<Game, ICollection<PlayerDto>>()
            .ConstructUsing(src => src.Players.Select(p => new PlayerDto
            {
                Name = p.Username,
                IsHost = src.HostId == p.Id
            }).ToList());
        CreateMap<RegisterDto, AppUser>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email));
    }
}
