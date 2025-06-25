using AutoMapper;
using MediatR;
using OnionArchitectureDemo.Application.Interfaces;
using OnionArchitectureDemo.Application.Wrappers;
using System;
using System.Threading;
using System.Threading.Tasks;
using YemekhaneApp.Application.DTOs.UserDebt;
using YemekhaneApp.Application.DTOs.UserDebt;
using YemekhaneApp.Application.Interfaces;
using YemekhaneApp.Domain.Entities;

using UserDebtEntity= YemekhaneApp.Domain.Entities.UserDebt;

namespace YemekhaneApp.Application.CQRS.Queries.UserDebts
{
    public class GetUserDebtByEmployeeIdAndMonth : IRequest<ServiceResponse<UserDebtDto>>
    {
        public Guid EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public GetUserDebtByEmployeeIdAndMonth(Guid employeeId, int year, int month)
        {
            EmployeeId = employeeId;
            Year = year;
            Month = month;
        }

        public class GetUserDebtByEmployeeIdAndMonthHandler : IRequestHandler<GetUserDebtByEmployeeIdAndMonth, ServiceResponse<UserDebtDto>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper _mapper;

            public GetUserDebtByEmployeeIdAndMonthHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<UserDebtDto>> Handle(GetUserDebtByEmployeeIdAndMonth request, CancellationToken cancellationToken)
            {
                var debt = await unitOfWork.GetRepository<UserDebtEntity>().GetAsync(
                    x => x.EmployeeId == request.EmployeeId && x.Year == request.Year && x.Month == request.Month);

                if (debt == null)
                    return new ServiceResponse<UserDebtDto>("Kayıt bulunamadı");

                var dto = _mapper.Map<UserDebtDto>(debt);
                return new ServiceResponse<UserDebtDto>(dto);
            }
        }
    }
}