using AutoMapper;
using MediatR;
using OnionArchitectureDemo.Application.Interfaces;
using OnionArchitectureDemo.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YemekhaneApp.Application.DTOs.UserDebt;
using YemekhaneApp.Application.Interfaces;
using YemekhaneApp.Domain.Entities;
using UserDebtEntity= YemekhaneApp.Domain.Entities.UserDebt;

namespace YemekhaneApp.Application.CQRS.Queries.UserDebt
{
    public class GetUserDebtsByEmployeeId : IRequest<ServiceResponse<List<UserDebtDto>>>
    {
        public Guid EmployeeId { get; set; }
        public GetUserDebtsByEmployeeId(Guid employeeId)
        {
            EmployeeId = employeeId;
        }

        public class GetUserDebtsByEmployeeIdHandler : IRequestHandler<GetUserDebtsByEmployeeId, ServiceResponse<List<UserDebtDto>>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper _mapper;

            public GetUserDebtsByEmployeeIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<List<UserDebtDto>>> Handle(GetUserDebtsByEmployeeId request, CancellationToken cancellationToken)
            {
                var debts = await unitOfWork.GetRepository<UserDebtEntity>().GetAllAsync(x => x.EmployeeId == request.EmployeeId);
                if (debts == null || !debts.Any())
                    return new ServiceResponse<List<UserDebtDto>>("Kayıt bulunamadı");

                var dtoList = _mapper.Map<List<UserDebtDto>>(debts);
                return new ServiceResponse<List<UserDebtDto>>(dtoList);
            }
        }
    }
}