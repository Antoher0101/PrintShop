using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShop.models
{
    [Table("ServiceInfo")]
    [Comment("Информация о возможных услугах PrintShop")]
    public class ServiceInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Format { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Paper { get; set; }
    }
}
