using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Domain.BaseEntities;

namespace YemekhaneApp.Domain.Entities
{
    public class Extra:EntityBase
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Navigation property: bir ekstra birden fazla mealrecord'da olabilir
        public ICollection<MealRecord> MealRecords { get; set; }
    }
}
