using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Application.DTOs.UserDebt;

namespace YemekhaneApp.Application.DTOs.Employee
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true; // Indicates if the employee is currently active
        public int TotalMealCount { get; set; } // Total number of meals consumed by the employee
        public List<UserDebtDto> UserDebts { get; set; } = new();
    }
}
