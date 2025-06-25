using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Application.Interfaces;

using UserDebtEntity= YemekhaneApp.Domain.Entities.UserDebt;

namespace YemekhaneApp.Application.CQRS.Commands.UserDebt
{
    public class DeleteUserDebtCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteUserDebtCommand(Guid id)
        {
            Id = id;
        }

        public class DeleteUserDebtCommandHandler : IRequestHandler<DeleteUserDebtCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteUserDebtCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteUserDebtCommand request, CancellationToken cancellationToken)
            {
                var repo = _unitOfWork.GetRepository<UserDebtEntity>();
                var entity = await repo.GetAsync(x => x.Id == request.Id);
                if (entity == null)
                    return false;

                repo.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();
                return true;
            }
        }
    }
}
