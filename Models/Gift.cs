using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{
    public class Gift
    {
        // Unique identifier for the gift
        [Key]
        public string GiftId { get; set; }
        /// Message to be limited to 200 characters
        [MaxLength(200)]
        public string Message { get; set; } = " ";


        /// sets the limit of the colour input hex value feild to be inforced by colour selector.
        [MaxLength(100)]
        public string RibbonColour { get; set; }

        [MaxLength(100)]
        public string BoxColour { get; set; }

        // Foreign key referencing the associated order (one to one)
        [ForeignKey("Order")]
        public string OrderId { get; set; }
        public Order Order { get; set; }

        //property for the recipient of the gift
        public GiftRecipient giftRecipient { get; set; }




    }
}
