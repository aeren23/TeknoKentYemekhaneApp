using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using OnionArchitectureDemo.Application.Wrappers;
using YemekhaneApp.Application.Interfaces;
using ExtraEntity = YemekhaneApp.Domain.Entities.Extra;

namespace YemekhaneApp.Application.CQRS.Commands.Extra
{
    public class DeleteExtraCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public DeleteExtraCommand(Guid id)
        {
            Id = id;
        }

        public class DeleteExtraCommandHandler : IRequestHandler<DeleteExtraCommand, ServiceResponse<bool>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public DeleteExtraCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ServiceResponse<bool>> Handle(DeleteExtraCommand request, CancellationToken cancellationToken)
            {
                var repo = _unitOfWork.GetRepository<ExtraEntity>();
                var extra = await repo.GetByGuidAsync(request.Id);
                if (extra == null)
                    return new ServiceResponse<bool>("Ekstra ürün bulunamadı.");

                await repo.DeleteAsync(extra);
                await _unitOfWork.SaveAsync();
                return new ServiceResponse<bool>(true);
            }
        }
    }
}