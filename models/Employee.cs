using Microsoft.EntityFrameworkCore;
using PrintShop.models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Documents;

namespace PrintShop.models
{
    [Table("Employee")]
    [Comment("Информация о сотруднике PrintShop")]
    public class Employee : Entity
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [InverseProperty("Employee")]
        public virtual List<TotalService> TotalServices { get; set; }

        public int CountService
        {
            get
            {
                int count = 0;
                foreach (var service in TotalServices)
                {
                    count += service.Services.Count;
                }
                return count;
            }
        }

        public override string ToString()
        {
            return $"{LastName} {Name} {MiddleName}";
        }
    }
}
