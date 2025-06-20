﻿namespace WebUI.Models.MealRecord
{
    public class MealRecordCreateViewModel
    {
        public Guid EmployeeId { get; set; } // Foreign key to Employee
        public DateOnly MealDate { get; set; } // Date of the meal
        public bool IsEaten { get; set; } // Indicates if the meal was eaten
    }
}
