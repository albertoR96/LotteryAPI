using AutoMapper;
using LotteryAPI.DTOs;
using LotteryAPI.Model;

namespace LotteryAPI.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AdministratorDTO, Administrator>();
            CreateMap<GameDTO, Game>();
            CreateMap<ParticipantDTO, Participant>();
            CreateMap<PrizeDTO, Prize>();
        }
    }
}
