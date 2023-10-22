using AutoMapper;
using Commander.Dtos;
using Commander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            //...Source -> Target
            //  Map between a source object & destination object - what we r mapping from and mapping to
            CreateMap<Command, CommandReadDto>();
            //        ^using Models; ^using Dtos;...Source -> Target

            CreateMap<CommandCreateDto, Command>();

            CreateMap<CommandUpdateDto, Command>();

            CreateMap<Command, CommandUpdateDto>();

        }
    }
}
