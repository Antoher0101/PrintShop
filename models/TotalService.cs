using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintShop.models.Base;

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
    }
}
