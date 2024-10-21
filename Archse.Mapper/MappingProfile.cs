using Archse.Events;
using Archse.Models;
using AutoMapper;

namespace Archse.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameRequest>();
            CreateMap<Game, GameResponse>();
            CreateMap<GameInsertedEvent, GameRequest>();
            CreateMap<GameRequest, GameInsertedEvent>();
            CreateMap<GameRequest, Game>();
        }
    }
}