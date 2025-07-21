using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OnionArchitectureDemo.Application.Wrappers;
using System.Threading;
using YemekhaneApp.Application.Interfaces;
using YemekhaneApp.Domain.Entities;
using MealRecordEntity= YemekhaneApp.Domain.Entities.MealRecord;

using YemekhaneApp.Application.DTOs.MealRecord;
using AutoMapper;

namespace YemekhaneApp.Application.CQRS.Queries.MealRecord
{
    public class GetMealRecordsByEmployeeIdQuery : IRequest<ServiceResponse<List<MealRecordDto>>>
    {
        public Guid EmployeeId { get; set; }
        public GetMealRecordsByEmployeeIdQuery(Guid employeeId)
        {
            EmployeeId = employeeId;
        }

        public class Handler : IRequestHandler<GetMealRecordsByEmployeeIdQuery, ServiceResponse<List<MealRecordDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<List<MealRecordDto>>> Handle(GetMealRecordsByEmployeeIdQuery request, CancellationToken cancellationToken)
            {
                var repo = _unitOfWork.GetRepository<MealRecordEntity>();
                var records = await repo.GetAllAsync(x => x.EmployeeId == request.EmployeeId,
                    e=>e.Extras
                    );
                if(records == null || !records.Any())
                {
                    return new ServiceResponse<List<MealRecordDto>>("No meal records found for the specified employee.");
                }
                var mappedRecords = _mapper.Map<List<MealRecordDto>>(records);
                return new ServiceResponse<List<MealRecordDto>>(mappedRecords);
            }
        }
    }
}
