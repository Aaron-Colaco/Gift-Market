using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{
    public class Gift
    {
        [Key]
        public int GiftId { get; set; }

        public string Message { get; set; }

        public string RibbonColour { get; set; }

        public string BoxColour { get; set; }

        [ForeignKey("Item")]
        public int OrderId { get; set; }
        public Order Order { get; set; }


        public GiftRecipient giftRecipient { get; set; }




    }
}
