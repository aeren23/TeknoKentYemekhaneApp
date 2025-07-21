using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Domain.BaseEntities;

namespace YemekhaneApp.Domain.Entities
{
    public class TrustedDevice:EntityBase
    {
        public string UserAgent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
