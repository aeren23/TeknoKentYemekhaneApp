using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OnionArchitectureDemo.Application.Wrappers;
using YemekhaneApp.Application.Interfaces;
using YemekhaneApp.Domain.Entities;
using AutoMapper;
using TrustedDeviceeNTİTY= YemekhaneApp.Domain.Entities.TrustedDevice;
using YemekhaneApp.Application.DTOs.TrustedDevice; // Adjust the namespace as per your project structure

namespace YemekhaneApp.Application.CQRS.Queries.TrustedDevice
{
    public class GetTrustedDevicesQuery : IRequest<ServiceResponse<List<TrustedDeviceDto>>>
    {
        public class GetTrustedDevicesQueryHandler : IRequestHandler<GetTrustedDevicesQuery, ServiceResponse<List<TrustedDeviceDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public GetTrustedDevicesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ServiceResponse<List<TrustedDeviceDto>>> Handle(GetTrustedDevicesQuery request, CancellationToken cancellationToken)
            {
                var devices = await _unitOfWork.GetRepository<TrustedDeviceeNTİTY>().GetAllAsync();
                if (devices == null || devices.Count == 0)
                    return new ServiceResponse<List<TrustedDeviceDto>>("Kayıtlı cihaz bulunamadı.");

                var mapped = _mapper.Map<List<TrustedDeviceDto>>(devices);
                return new ServiceResponse<List<TrustedDeviceDto>>(mapped);
            }
        }
    }
}
