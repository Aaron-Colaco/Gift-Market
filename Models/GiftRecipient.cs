using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{
    public class GiftRecipient
    {
        [Key]
        public string RecipientId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public  string Address { get; set; }

        public string City { get; set; }

        [ForeignKey("Gift")]
        public string GiftId { get; set; }
        public Gift Gift { get; set; }
       


    }
}
