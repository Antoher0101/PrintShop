using Microsoft.EntityFrameworkCore;
using PrintShop.models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShop.models
{
    [Table("Service")]
    [Comment("Содержит информацию о предоставляемой услуге и количество страниц")]
    public class Service : Entity
    {
        public int Count { get; set; }

        [ForeignKey("IdTotalService")]
        public virtual TotalService TotalService { get; set; }

        public int IdTotalService { get; set; }

        [ForeignKey("IdService")]
        public virtual ServiceInfo ServiceInfo { get; set; }

        public int IdService { get; set; }

        [NotMapped]
        public double TotalPrice { get => Count * ServiceInfo.Price; }
    }
}
