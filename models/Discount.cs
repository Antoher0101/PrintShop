using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShop.models
{
    [Table("Discout")]
    [Comment("Информация о скидке, предоставляемой клиенту")]
    public class Discount
    {
        [Required]
        [ForeignKey("idClient")]
        public Client Client { get; set; }

        [Required]
        [ForeignKey("idDiscount")]
        public DiscountInfo DiscountInfo { get; set; }

        public override string ToString()
        {
            return $"{Client}\t({DiscountInfo})";
        }
    }
}
