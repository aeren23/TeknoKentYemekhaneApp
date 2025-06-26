using AutoMapper;
using MediatR;
using OnionArchitectureDemo.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YemekhaneApp.Application.DTOs.MealRecord;
using YemekhaneApp.Application.Interfaces;
using MealRecordEntity = YemekhaneApp.Domain.Entities.MealRecord;

namespace YemekhaneApp.Application.CQRS.Queries.MealRecord
{
    public class GetAllMealRecordsQuery : IRequest<ServiceResponse<List<MealRecordDto>>>
    {
        public class GetAllMealRecordsQueryHandler : IRequestHandler<GetAllMealRecordsQuery, ServiceResponse<List<MealRecordDto>>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper _mapper;

            public GetAllMealRecordsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<List<MealRecordDto>>> Handle(GetAllMealRecordsQuery request, CancellationToken cancellationToken)
            {
                // Extras ve Employee navigation property'lerini dahil et
                var records = await unitOfWork
                    .GetRepository<MealRecordEntity>()
                    .GetAllAsync(null, x => x.Extras, x => x.Employee);

                if (records == null || !records.Any())
                    return new ServiceResponse<List<MealRecordDto>>("Kayıt bulunamadı");

                var dtoList = _mapper.Map<List<MealRecordDto>>(records);

                return new ServiceResponse<List<MealRecordDto>>(dtoList);
            }
        }
    }
}
