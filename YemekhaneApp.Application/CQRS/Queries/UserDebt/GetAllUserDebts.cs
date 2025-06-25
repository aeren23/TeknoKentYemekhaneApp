using AutoMapper;
using MediatR;
using OnionArchitectureDemo.Application.Interfaces;
using OnionArchitectureDemo.Application.Wrappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YemekhaneApp.Application.DTOs.UserDebt;
using YemekhaneApp.Application.Interfaces;
using YemekhaneApp.Domain.Entities;
using UserDebtEntity= YemekhaneApp.Domain.Entities.UserDebt;

namespace YemekhaneApp.Application.CQRS.Queries.UserDebts
{
    public class GetAllUserDebts : IRequest<ServiceResponse<List<UserDebtDto>>>
    {
        public class GetAllUserDebtsHandler : IRequestHandler<GetAllUserDebts, ServiceResponse<List<UserDebtDto>>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper _mapper;

            public GetAllUserDebtsHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<List<UserDebtDto>>> Handle(GetAllUserDebts request, CancellationToken cancellationToken)
            {
                var debts = await unitOfWork.GetRepository<UserDebtEntity>().GetAllAsync();
                if (debts == null || !debts.Any())
                    return new ServiceResponse<List<UserDebtDto>>("Kayıt bulunamadı");

                var dtoList = _mapper.Map<List<UserDebtDto>>(debts);
                return new ServiceResponse<List<UserDebtDto>>(dtoList);
            }
        }
    }
}