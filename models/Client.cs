using Microsoft.EntityFrameworkCore;
using PrintShop.models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Documents;

namespace PrintShop.models
{
    [Table("Client")]
    [Comment("Информация о клиенте")]
    public class Client : Entity
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Phone { get; set; }

        [InverseProperty("Client")]
        public virtual List<Discount> Discounts { get; set; }

        [InverseProperty("Client")]
        public virtual List<TotalService> TotalServices { get; set; }

        public override string ToString() => $"{Id} - {Name} {LastName} {MiddleName} {Phone}";
    }
}
