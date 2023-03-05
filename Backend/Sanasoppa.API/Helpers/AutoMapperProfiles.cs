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
            CreateMap<Player, PlayerDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.IsDasher,
                    opt => opt.MapFrom(src => src.IsDasher));
            CreateMap<Score, ScoreDto>()
                .ForMember(dest => dest.PlayerName,
                    opt => opt.MapFrom(src => src.Player.Username))
                .ForMember(dest => dest.TotalPoints, opt => opt.Ignore())
                .AfterMap((src, dest) => {
                    dest.TotalPoints = src.Player.Scores.Where(s => s.Round.GameId == src.Round.GameId).Sum(s => s.Points);
                });
        }
    }
}
