using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShop.models
{
    [Table("DiscountInfo")]
    [Comment("Информация о существующих скидках")]
    public class DiscountInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public int Percent { get; set; }

        [Required]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Percent}%";
        }
    }
}
