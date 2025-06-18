﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Domain.BaseEntities;

namespace YemekhaneApp.Domain.Entities
{
    public class MealRecord:EntityBase
    {
        public Guid EmployeeId { get; set; } // Foreign key to Employee
        public DateOnly MealDate { get; set; } // Date of the meal
        public bool IsEaten { get; set; } // Indicates if the meal was eaten
        public Employee Employee { get; set; }
    }
}
