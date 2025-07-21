using AutoMapper;
using MediatR;
using OnionArchitectureDemo.Application.Interfaces;
using OnionArchitectureDemo.Application.Wrappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YemekhaneApp.Application.DTOs.Employee;
using YemekhaneApp.Application.Interfaces;
using YemekhaneApp.Domain.Entities;
using UserDebtEntity= YemekhaneApp.Domain.Entities.UserDebt;

namespace YemekhaneApp.Application.CQRS.Queries.UserDebts
{
    public class GetEmployeesWithUserDebtQuery : IRequest<ServiceResponse<List<EmployeeDto>>>
    {
        public class GetEmployeesWithUserDebtQueryHandler : IRequestHandler<GetEmployeesWithUserDebtQuery, ServiceResponse<List<EmployeeDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public GetEmployeesWithUserDebtQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<List<EmployeeDto>>> Handle(GetEmployeesWithUserDebtQuery request, CancellationToken cancellationToken)
            {
                // UserDebt tablosunda kaydı olan EmployeeId'leri bul
                var userDebtRepo = _unitOfWork.GetRepository<UserDebtEntity>();
                var employeeRepo = _unitOfWork.GetRepository<Employee>();

                var userDebts = await userDebtRepo.GetAllAsync();
                var employeeIdsWithDebt = userDebts.Select(ud => ud.EmployeeId).Distinct().ToList();

                if (!employeeIdsWithDebt.Any())
                    return new ServiceResponse<List<EmployeeDto>>("Rapor kaydı olan kullanıcı bulunamadı.");

                // Sadece bu Id'lere sahip çalışanları getir
                var employees = await employeeRepo.GetAllAsync(e => employeeIdsWithDebt.Contains(e.Id),
                e => e.UserDebts
                );
                var dtoList = _mapper.Map<List<EmployeeDto>>(employees);

                return new ServiceResponse<List<EmployeeDto>>(dtoList);
            }
        }
    }
}