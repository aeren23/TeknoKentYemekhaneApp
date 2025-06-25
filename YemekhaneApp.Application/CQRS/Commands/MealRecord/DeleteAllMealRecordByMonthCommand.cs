using MediatR;
using OnionArchitectureDemo.Application.Wrappers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YemekhaneApp.Application.Interfaces;
using MealRecordEntity = YemekhaneApp.Domain.Entities.MealRecord;
using EmployeeEntity = YemekhaneApp.Domain.Entities.Employee;

namespace YemekhaneApp.Application.CQRS.Commands.MealRecord
{
    public class DeleteAllMealRecordByMonthCommand : IRequest<bool>
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public DeleteAllMealRecordByMonthCommand(int month, int year)
        {
            Month = month;
            Year = year;
        }

        public class DeleteAllMealRecordByMonthCommandHandler : IRequestHandler<DeleteAllMealRecordByMonthCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            public DeleteAllMealRecordByMonthCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteAllMealRecordByMonthCommand request, CancellationToken cancellationToken)
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    var mealRecordRepo = _unitOfWork.GetRepository<MealRecordEntity>();
                    var employeeRepo = _unitOfWork.GetRepository<EmployeeEntity>();

                    // Belirtilen ay ve yýldaki tüm yemek kayýtlarýný al
                    var mealRecords = await mealRecordRepo.GetAllAsync(m => m.Month == request.Month && m.Year == request.Year);

                    if (mealRecords == null || !mealRecords.Any())
                        return false; // Kayýt yoksa

                    // Ýlgili çalýþanlarýn Id'lerini al
                    var employeeIds = mealRecords.Select(m => m.EmployeeId).Distinct().ToList();

                    // Ýlgili çalýþanlarý getir
                    var employees = await employeeRepo.GetAllAsync(e => employeeIds.Contains(e.Id));

                    // Her çalýþanýn TotalMealCount'unu güncelle
                    foreach (var employee in employees)
                    {
                        employee.TotalMealCount = 0;
                        await employeeRepo.UpdateAsync(employee);
                    }

                    // Tüm meal record'larý sil
                    foreach (var mealRecord in mealRecords)
                    {
                        await mealRecordRepo.DeleteAsync(mealRecord);
                    }

                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}