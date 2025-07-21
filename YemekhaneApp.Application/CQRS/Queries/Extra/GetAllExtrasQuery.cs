using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OnionArchitectureDemo.Application.Wrappers;
using YemekhaneApp.Application.Interfaces;
using ExtraEntity = YemekhaneApp.Domain.Entities.Extra;
using YemekhaneApp.Application.DTOs.Extra;
using AutoMapper;

namespace YemekhaneApp.Application.CQRS.Queries.Extra
{
    public class GetAllExtrasQuery : IRequest<ServiceResponse<List<ExtraDto>>>
    {
        public class GetAllExtrasQueryHandler : IRequestHandler<GetAllExtrasQuery, ServiceResponse<List<ExtraDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper mapper;
            public GetAllExtrasQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                this.mapper = mapper;
            }

            public async Task<ServiceResponse<List<ExtraDto>>> Handle(GetAllExtrasQuery request, CancellationToken cancellationToken)
            {
                var extras = await _unitOfWork.GetRepository<ExtraEntity>().GetAllAsync();
                if (extras == null || extras.Count == 0)
                    return new ServiceResponse<List<ExtraDto>>("Ekstra ürün bulunamadı.");
                var map=mapper.Map<List<ExtraDto>>(extras);

                return new ServiceResponse<List<ExtraDto>>(map);
            }
        }
    }
}