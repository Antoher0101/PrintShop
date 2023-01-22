using Microsoft.EntityFrameworkCore;
using PrintShop.models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShop.models
{
    [Table("ServiceInfo")]
    [Comment("Информация о возможных услугах PrintShop")]
    public class ServiceInfo : Entity
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Format { get; set; }

        public string Type { get; set; }

        public string Paper { get; set; }

        public override string ToString() => $"{Name} {Format} - {Type}({Paper})";
    }
}
