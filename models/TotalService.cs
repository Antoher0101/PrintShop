using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.models
{
    [Table("TotalService")]
    public class TotalService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("idClient")]
        public Client Client { get; set; }

        [Required]
        [ForeignKey("idDiscount")]
        public Discount Discount { get; set; }

        [Required]
        public string Date { get; set; }
    }
}
