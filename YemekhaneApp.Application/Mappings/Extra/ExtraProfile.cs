using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemekhaneApp.Application.Mappings.Extra
{
    public class ExtraProfile :Profile
    {
        public ExtraProfile()
        {
            CreateMap<Domain.Entities.Extra, DTOs.Extra.ExtraDto>().ReverseMap();
            CreateMap<Domain.Entities.Extra, CQRS.Commands.Extra.CreateExtraCommand>().ReverseMap();
            CreateMap<Domain.Entities.Extra, CQRS.Commands.Extra.UpdateExtraCommand>().ReverseMap();
        }
    }
}
