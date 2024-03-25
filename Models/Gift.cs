using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{
    public class Gift
    {
        [Key]
        public string GiftId { get; set; }

        [MaxLength(200)]
        public string Message { get; set; }

        [MaxLength(100)]
        public string RibbonColour { get; set; }

        [MaxLength(100)]
        public string BoxColour { get; set; }

        [ForeignKey("Item")]
        public string OrderId { get; set; }
        public Order Order { get; set; }


        public GiftRecipient giftRecipient { get; set; }




    }
}
