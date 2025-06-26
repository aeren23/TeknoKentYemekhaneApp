using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using YemekhaneApp.Application.Interfaces;

namespace YemekhaneApp.Application.CQRS.Commands.UserDebt
{
    public class UpdateUserDebtCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }

        public UpdateUserDebtCommand(Guid id, decimal amount, bool isPaid)
        {
            Id = id;
            Amount = amount;
            IsPaid = isPaid;
        }

        public class UpdateUserDebtCommandHandler : IRequestHandler<UpdateUserDebtCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            public UpdateUserDebtCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateUserDebtCommand request, CancellationToken cancellationToken)
            {
                var repo = _unitOfWork.GetRepository<Domain.Entities.UserDebt>();
                var userDebt = await repo.GetByGuidAsync(request.Id);
                if (userDebt == null)
                    return false;

                userDebt.Amount = request.Amount;
                userDebt.IsPaid = request.IsPaid;

                await repo.UpdateAsync(userDebt);
                await _unitOfWork.SaveAsync();
                return true;
            }
        }
    }
}