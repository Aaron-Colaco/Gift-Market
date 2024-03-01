using System.ComponentModel.DataAnnotations;

namespace AaronColacoAsp.NETProject.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
    
        public DateTime OrderTime { get; set; }

        public int TotalPrice { get; set; }





    }
}
