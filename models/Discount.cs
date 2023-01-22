using PrintShop.models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShop.models
{
    [Table("Discount")]
    public class Discount : Entity
    {
        public int IdClient { get; set; }

        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; }
        
        public int IdDiscount { get; set; }

        [ForeignKey("IdDiscount")]
        public virtual DiscountInfo DiscountInfo { get; set; }
    }
}
