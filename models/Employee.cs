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

        public override string ToString()
        {
            return $"{Id} - {Name} {LastName} {MiddleName}";
        }
    }
}
