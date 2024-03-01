using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
    
        public DateTime OrderTime { get; set; }

        public int TotalPrice { get; set; }

        public Gift Gifts { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public Status Status { get; set; }



    }
}
