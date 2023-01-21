using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShop.models
{
    [Table("Service")]
    [Comment("Содержит информацию о предоставляемой услуге и количество страниц")]
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        [ForeignKey("idEmployee")]
        public Employee Employee { get; set; }

        [Required]
        [ForeignKey("idTotalService")]
        public TotalService TotalService { get; set; }

        [Required]
        [ForeignKey("idService")]
        public ServiceInfo ServiceInfo { get; set; }
    }
}
