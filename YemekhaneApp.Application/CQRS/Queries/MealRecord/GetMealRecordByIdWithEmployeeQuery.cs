using AutoMapper;
using MediatR;
using OnionArchitectureDemo.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Application.DTOs.MealRecord;
using YemekhaneApp.Application.Interfaces;
using MealRecordEntity = YemekhaneApp.Domain.Entities.MealRecord;

namespace YemekhaneApp.Application.CQRS.Queries.MealRecord
{
    public class GetMealRecordByIdWithEmployeeQuery : IRequest<ServiceResponse<List<MealRecordDto>>>
    {
        public Guid Id { get; set; }

        public GetMealRecordByIdWithEmployeeQuery(Guid id)
        {
            Id = id;
        }

        public class GetMealRecordByIdWithEmployeeQueryHandler : IRequestHandler<GetMealRecordByIdWithEmployeeQuery, ServiceResponse<List<MealRecordDto>>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper _mapper;

            public GetMealRecordByIdWithEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<List<MealRecordDto>>> Handle(GetMealRecordByIdWithEmployeeQuery request, CancellationToken cancellationToken)
            {
                var records = await unitOfWork.GetRepository<MealRecordEntity>().GetAllAsync(
                    m => m.Id==request.Id,
                    x => x.Employee,// Include Employee details
                    e => e.Extras
                );
                if(records == null || !records.Any())
                    return new ServiceResponse<List<MealRecordDto>>("Kayıt bulunamadı");


                var dtoList = _mapper.Map<List<MealRecordDto>>(records);

                return new ServiceResponse<List<MealRecordDto>>(dtoList);
            }
        }
    }
}
