using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using OnionArchitectureDemo.Application.Wrappers;
using YemekhaneApp.Application.Interfaces;
using ExtraEntity = YemekhaneApp.Domain.Entities.Extra;
using YemekhaneApp.Application.DTOs.Extra;
using AutoMapper;

namespace YemekhaneApp.Application.CQRS.Queries.Extra
{
    public class GetExtraByIdQuery : IRequest<ServiceResponse<ExtraDto>>
    {
        public Guid Id { get; set; }
        public GetExtraByIdQuery(Guid id)
        {
            Id = id;
        }

        public class GetExtraByIdQueryHandler : IRequestHandler<GetExtraByIdQuery, ServiceResponse<ExtraDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper mapper;
            public GetExtraByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                this.mapper = mapper;
            }

            public async Task<ServiceResponse<ExtraDto>> Handle(GetExtraByIdQuery request, CancellationToken cancellationToken)
            {
                var extra = await _unitOfWork.GetRepository<ExtraEntity>().GetByGuidAsync(request.Id);
                if (extra == null)
                    return new ServiceResponse<ExtraDto>("Ekstra ürün bulunamadı.");

                var map=mapper.Map<ExtraDto>(extra);
                return new ServiceResponse<ExtraDto>(map);
            }
        }
    }
}