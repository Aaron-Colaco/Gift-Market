using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderId { get; set; }


        [Required]
        public DateTime OrderTime { get; set; } = DateTime.Now;

        [DataType(DataType.Currency),Range(0,1000)]
        public decimal TotalPrice { get; set; }

        public Gift Gifts { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        [Required, MaxLength(100)]
        public string DeliveryAddress { get; set; } = "";

        [Required, MaxLength(50)]
        public string City { get; set; }= "";

        [Required, DataType(DataType.PostalCode)]
        public int PostalCode { get; set; }


        [ForeignKey("Status")]
        public int StatusId { get; set; } = 1;
        public Status Status { get; set; }


        [ForeignKey("Customer")]
        public string  CustomerId { get; set; }

        public Customer Customers { get; set; }

    }
}
