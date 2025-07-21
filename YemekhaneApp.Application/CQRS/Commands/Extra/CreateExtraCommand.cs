using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using OnionArchitectureDemo.Application.Wrappers;
using YemekhaneApp.Application.Interfaces;
using ExtraEntity = YemekhaneApp.Domain.Entities.Extra;

namespace YemekhaneApp.Application.CQRS.Commands.Extra
{
    public class CreateExtraCommand : IRequest<ServiceResponse<Guid>>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public CreateExtraCommand(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public class CreateExtraCommandHandler : IRequestHandler<CreateExtraCommand, ServiceResponse<Guid>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CreateExtraCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ServiceResponse<Guid>> Handle(CreateExtraCommand request, CancellationToken cancellationToken)
            {
                var extra = new ExtraEntity
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Price = request.Price
                };
                await _unitOfWork.GetRepository<ExtraEntity>().AddAsync(extra);
                await _unitOfWork.SaveAsync();
                return new ServiceResponse<Guid>(extra.Id);
            }
        }
    }
}