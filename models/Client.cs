using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShop.models
{
    [Table("Client")]
    [Comment("Информация о клиенте")]
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string Phone { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Name} {LastName} {MiddleName} {Phone}";
        }
    }
}
