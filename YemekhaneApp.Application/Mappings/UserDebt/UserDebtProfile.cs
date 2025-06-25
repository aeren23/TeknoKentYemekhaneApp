using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemekhaneApp.Application.Mappings.UserDebt
{
    public class UserDebtProfile : Profile
    {
        public UserDebtProfile()
        {
            CreateMap<Domain.Entities.UserDebt, DTOs.UserDebt.UserDebtDto>().ReverseMap();
            CreateMap<Domain.Entities.UserDebt, CQRS.Commands.UserDebt.CreateUserDebtCommand>().ReverseMap();
        }
    }
}
