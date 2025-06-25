using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Domain.BaseEntities;

namespace YemekhaneApp.Domain.Entities
{
    public class UserDebt:EntityBase
    {
        public Guid EmployeeId { get; set; } // Foreign key to Employee
        public int Year { get; set; }
        public int Month { get; set; } 
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; } 
    }
}
