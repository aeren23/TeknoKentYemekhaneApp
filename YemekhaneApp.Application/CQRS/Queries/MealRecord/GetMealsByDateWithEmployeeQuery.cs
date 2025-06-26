using AutoMapper;
using MediatR;
using OnionArchitectureDemo.Application.Interfaces;
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
    public class GetMealsByDateWithEmployeeQuery : IRequest<ServiceResponse<List<MealRecordDto>>>
    {
        public Guid Id { get; set; }

        public GetMealsByDateWithEmployeeQuery(Guid id)
        {
            Id = id;
        }

        public class GetMealsByDateWithEmployeeQueryHandler : IRequestHandler<GetMealsByDateWithEmployeeQuery, ServiceResponse<List<MealRecordDto>>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper _mapper;

            public GetMealsByDateWithEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<List<MealRecordDto>>> Handle(GetMealsByDateWithEmployeeQuery request, CancellationToken cancellationToken)
            {
                // Önce id'ye ait meal record'u bul
                var mealRecordRepo = unitOfWork.GetRepository<MealRecordEntity>();
                var targetRecord = await mealRecordRepo.GetByGuidAsync(request.Id);

                if (targetRecord == null)
                    return new ServiceResponse<List<MealRecordDto>>("Kayıt bulunamadı");

                var mealDate = targetRecord.MealDate;

                // Aynı tarihteki tüm meal record'ları, Employee ile birlikte getir
                var records = await mealRecordRepo.GetAllAsync(
                    m => m.MealDate == mealDate,
                    x => x.Employee,
                    e => e.Extras
                );

                if (records == null || !records.Any())
                    return new ServiceResponse<List<MealRecordDto>>("Kayıt bulunamadı");

                var dtoList = _mapper.Map<List<MealRecordDto>>(records);

                return new ServiceResponse<List<MealRecordDto>>(dtoList);
            }
        }
    }

}
