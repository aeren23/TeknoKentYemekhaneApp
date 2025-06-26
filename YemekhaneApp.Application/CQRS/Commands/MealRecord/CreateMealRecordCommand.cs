using AutoMapper;
using MediatR;
using OnionArchitectureDemo.Application.Interfaces;
using OnionArchitectureDemo.Application.Wrappers;
using System;
using System.Threading;
using System.Threading.Tasks;
using YemekhaneApp.Application.Interfaces;
using YemekhaneApp.Domain.Entities;
using MealRecordEntity = YemekhaneApp.Domain.Entities.MealRecord;
using EmployeeEntity = YemekhaneApp.Domain.Entities.Employee;

namespace YemekhaneApp.Application.CQRS.Commands.MealRecord
{
    public class CreateMealRecordCommand : IRequest<ServiceResponse<Guid>>
    {
        public Guid EmployeeId { get; set; }
        public DateOnly MealDate { get; set; }
        public bool IsEaten { get; set; }


        public class CreateMealRecordCommandHandler : IRequestHandler<CreateMealRecordCommand, ServiceResponse<Guid>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public CreateMealRecordCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<Guid>> Handle(CreateMealRecordCommand request, CancellationToken cancellationToken)
            {
                var mealRecordRepository = _unitOfWork.GetRepository<MealRecordEntity>();
                var employeeRepository = _unitOfWork.GetRepository<EmployeeEntity>();

                using var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    var employee = await employeeRepository.GetByGuidAsync(request.EmployeeId);
                    if (employee == null)
                        return new ServiceResponse<Guid>("Employee not found");

                    var existingRecord = (await mealRecordRepository.GetAllAsync(
                        m => m.EmployeeId == request.EmployeeId && m.MealDate == request.MealDate)).FirstOrDefault();

                    if (existingRecord != null)
                        return new ServiceResponse<Guid>("Meal record already exists for this date.");


                    var mealRecord = _mapper.Map<MealRecordEntity>(request);

                    mealRecord.Year = request.MealDate.Year;
                    mealRecord.Month = request.MealDate.Month;

                    if (mealRecord.IsEaten)
                        employee.TotalMealCount++;

                    await mealRecordRepository.AddAsync(mealRecord);
                    await employeeRepository.UpdateAsync(employee);

                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();

                    return new ServiceResponse<Guid>(mealRecord.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceResponse<Guid>($"Error creating meal record: {ex.Message}");
                }
            }
        }
    }
}