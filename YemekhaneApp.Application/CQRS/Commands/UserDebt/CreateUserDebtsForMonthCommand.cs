using MediatR;
using YemekhaneApp.Application.CQRS.Commands.UserDebt;
using YemekhaneApp.Application.Interfaces;
using YemekhaneApp.Domain.Entities;

public class CreateUserDebtsForMonthCommand : IRequest<bool>
{
    public int Year { get; }
    public int Month { get; }

    public CreateUserDebtsForMonthCommand(int year, int month)
    {
        Year = year;
        Month = month;
    }

    public class Handler : IRequestHandler<CreateUserDebtsForMonthCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private const decimal MenuPrice = 150m;

        public Handler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<bool> Handle(CreateUserDebtsForMonthCommand request, CancellationToken cancellationToken)
        {
            // 1. O ayın tüm mealRecord'larını çek
            var mealRecords = await _unitOfWork.GetRepository<MealRecord>().GetAllAsync(
                x => x.MealDate.Year == request.Year && x.MealDate.Month == request.Month,
                e => e.Extras
            );

            // 2. Kullanıcı bazında grupla ve borçları hesapla
            var debts = mealRecords
                .GroupBy(mr => mr.EmployeeId)
                .Select(g =>
                {
                    var total = g.Count() * MenuPrice + g.SelectMany(mr => mr.Extras ?? new List<Extra>()).Sum(e => e.Price);
                    return new
                    {
                        EmployeeId = g.Key,
                        Amount = total
                    };
                })
                .ToList();

            // 3. Her kullanıcı için borç kaydı oluştur
            foreach (var debt in debts)
            {
                var command = new CreateUserDebtCommand(debt.EmployeeId, request.Year, request.Month, debt.Amount);
                await _mediator.Send(command, cancellationToken);

                // Employee'nin meal count'unu sıfırla
                var employee = await _unitOfWork.GetRepository<Employee>().GetByGuidAsync(debt.EmployeeId);
                if (employee != null)
                {
                    employee.TotalMealCount = 0;
                    await _unitOfWork.GetRepository<Employee>().UpdateAsync(employee);
                    await _unitOfWork.SaveAsync();
                }
            }

            return true;
        }
    }
}