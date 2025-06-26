namespace YemekhaneApp.Frontend.Models.MealRecord
{
    public class MealRecordCreateViewModel
    {
        public Guid EmployeeId { get; set; } // Foreign key to Employee
        public DateOnly MealDate { get; set; } // Date of the meal
        public bool IsEaten { get; set; }
        public List<Guid> ExtraIds { get; set; } = new();
    }
}
