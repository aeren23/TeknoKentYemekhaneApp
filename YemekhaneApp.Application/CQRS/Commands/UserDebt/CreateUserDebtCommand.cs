using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Application.Interfaces;

namespace YemekhaneApp.Application.CQRS.Commands.UserDebt
{
    public class CreateUserDebtCommand : IRequest<Guid>
    {
        public Guid EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }

        public CreateUserDebtCommand(Guid employeeId, int year, int month, decimal amount)
        {
            EmployeeId = employeeId;
            Year = year;
            Month = month;
            Amount = amount;
        }
        public class CreateUserDebtCommandHandler : IRequestHandler<CreateUserDebtCommand, Guid>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CreateUserDebtCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<Guid> Handle(CreateUserDebtCommand request, CancellationToken cancellationToken)
            {
                var userDebt = new Domain.Entities.UserDebt
                {
                    EmployeeId = request.EmployeeId,
                    Year = request.Year,
                    Month = request.Month,
                    Amount = request.Amount,
                    IsPaid = false
                };
                await _unitOfWork.GetRepository<Domain.Entities.UserDebt>().AddAsync(userDebt);
                await _unitOfWork.SaveAsync();
                return userDebt.Id;
            }
        }
    }
}
