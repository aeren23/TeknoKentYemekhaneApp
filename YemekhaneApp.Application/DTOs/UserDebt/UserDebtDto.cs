using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Application.DTOs.Employee;

namespace YemekhaneApp.Application.DTOs.UserDebt
{
    public class UserDebtDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; } // Foreign key to Employee
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public EmployeeDto Employee { get; set; } // Navigation property to Employee
    }
}
