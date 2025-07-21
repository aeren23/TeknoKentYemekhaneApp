namespace YemekhaneApp.Frontend.Models.MealRecord
{
    public class MealRecordUpdateViewModel
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateOnly MealDate { get; set; }
        public bool IsEaten { get; set; }
        public List<Guid> ExtraIds { get; set; } = new();
    }
}
