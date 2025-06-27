using YemekhaneApp.Frontend.Models.UserDebt;

namespace YemekhaneApp.Frontend.Models.Employee
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public int TotalMealCount { get; set; }
        public List<UserDebtViewModel> UserDebts { get; set; } = new();
    }
}
