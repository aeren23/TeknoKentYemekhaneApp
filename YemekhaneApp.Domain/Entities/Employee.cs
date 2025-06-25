using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Domain.BaseEntities;

namespace YemekhaneApp.Domain.Entities
{
    public class Employee:EntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true; // Indicates if the employee is currently active

        public int TotalMealCount { get; set; }
        public ICollection<MealRecord> MealRecords { get; set; }
        public ICollection<UserDebt> UserDebts { get; set; } 
    }
}
