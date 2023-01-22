using PrintShop.models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShop.models
{
    [Table("DiscountInfo")]
    public class DiscountInfo : Entity
    {
        public int Percent { get; set; }

        public string Name { get; set; }

        public override string ToString() => $"({Name}) {Percent}%";
    }
}