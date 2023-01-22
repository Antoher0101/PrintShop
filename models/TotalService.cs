using PrintShop.models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShop.models
{
    [Table("TotalService")]
    public class TotalService : Entity
    {
        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; }

        public int IdClient { get; set; }

        [ForeignKey("IdEmployee")]
        public virtual Employee Employee { get; set; }

        public int IdEmployee { get; set; }

        public string Date { get; set; }

        [InverseProperty("TotalService")]
        public virtual List<Service> Services { get; set; }

        public int IdDiscount { get; set; }

        [ForeignKey("IdDiscount")]
        public virtual Discount Discount { get; set; }

        [NotMapped]
        public double TotalPriceWithDiscount
        {
            get
            {
                double total = 0;
                int percent = Discount.DiscountInfo.Percent;
                foreach (Service s in Services)
                    total += s.TotalPrice;
                if (percent == 0) return total;
                else
                    return total * percent / 100;
            }
        }

        public override string ToString() => this.Id.ToString();
    }
}
