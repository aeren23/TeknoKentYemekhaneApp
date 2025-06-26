using YemekhaneApp.Frontend.Models.Employee;
using YemekhaneApp.Frontend.Models.Extra;

namespace YemekhaneApp.Frontend.Models.MealRecord
{
    public class MealRecordViewModel
    {
        public Guid Id { get; set; } 
        public Guid EmployeeId { get; set; } 
        public DateOnly MealDate { get; set; } 
        public bool IsEaten { get; set; } 
        public EmployeeViewModel Employee { get; set; }
        public List<ExtraViewModel> Extras { get; set; } = new();

    }
}
