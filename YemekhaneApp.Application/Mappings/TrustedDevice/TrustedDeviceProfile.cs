using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemekhaneApp.Application.Mappings.TrustedDevice
{
    public class TrustedDeviceProfile:Profile
    {
        public TrustedDeviceProfile()
        {
            CreateMap<Domain.Entities.TrustedDevice, DTOs.TrustedDevice.TrustedDeviceDto>()
                .ReverseMap();
        }
    }
}
