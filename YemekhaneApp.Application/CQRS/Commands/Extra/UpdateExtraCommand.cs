using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using OnionArchitectureDemo.Application.Wrappers;
using YemekhaneApp.Application.Interfaces;
using ExtraEntity = YemekhaneApp.Domain.Entities.Extra;

namespace YemekhaneApp.Application.CQRS.Commands.Extra
{
    public class UpdateExtraCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public UpdateExtraCommand(Guid id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public class UpdateExtraCommandHandler : IRequestHandler<UpdateExtraCommand, ServiceResponse<bool>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public UpdateExtraCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ServiceResponse<bool>> Handle(UpdateExtraCommand request, CancellationToken cancellationToken)
            {
                var repo = _unitOfWork.GetRepository<ExtraEntity>();
                var extra = await repo.GetByGuidAsync(request.Id);
                if (extra == null)
                    return new ServiceResponse<bool>("Ekstra ürün bulunamadı.");

                extra.Name = request.Name;
                extra.Price = request.Price;
                await repo.UpdateAsync(extra);
                await _unitOfWork.SaveAsync();
                return new ServiceResponse<bool>(true);
            }
        }
    }
}