using YemekhaneApp.Frontend.Models.Employee;

namespace YemekhaneApp.Frontend.Models.UserDebt
{
    public class UserDebtViewModel
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; } // Foreign key to Employee
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public EmployeeViewModel Employee { get; set; } // Navigation property to Employee
    }
}
