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
    public class UpdateMealRecordCommand : IRequest<ServiceResponse<Guid>>
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateOnly MealDate { get; set; }
        public bool IsEaten { get; set; }

        public class UpdateMealRecordCommandHandler : IRequestHandler<UpdateMealRecordCommand, ServiceResponse<Guid>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public UpdateMealRecordCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<Guid>> Handle(UpdateMealRecordCommand request, CancellationToken cancellationToken)
            {
                var mealRecordRepository = _unitOfWork.GetRepository<MealRecordEntity>();
                var employeeRepository = _unitOfWork.GetRepository<EmployeeEntity>();

                using var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    var employee = await employeeRepository.GetByGuidAsync(request.EmployeeId);
                    if (employee == null)
                        return new ServiceResponse<Guid>("Employee not found");

                    var existingRecord = await mealRecordRepository.GetByGuidAsync(request.Id);

                    if (existingRecord == null)
                        return new ServiceResponse<Guid>("Meal record not found.");

                    // Sadece IsEaten deðeri deðiþirse meal count güncellenir
                    if (existingRecord.IsEaten != request.IsEaten)
                    {
                        if (request.IsEaten)
                        {
                            employee.TotalMealCount++;
                        }
                        else
                        {
                            if (employee.TotalMealCount > 0)
                                employee.TotalMealCount--;
                        }
                    }

                    existingRecord.IsEaten = request.IsEaten;
                    existingRecord.MealDate = request.MealDate;
                    existingRecord.EmployeeId = request.EmployeeId;
                    existingRecord.Year = request.MealDate.Year;
                    existingRecord.Month = request.MealDate.Month;

                    await mealRecordRepository.UpdateAsync(existingRecord);
                    await employeeRepository.UpdateAsync(employee);

                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();

                    return new ServiceResponse<Guid>(existingRecord.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceResponse<Guid>($"Error updating meal record: {ex.Message}");
                }
            }
        }
    }
}