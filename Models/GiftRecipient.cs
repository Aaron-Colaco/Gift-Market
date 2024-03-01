using System.ComponentModel.DataAnnotations;

namespace AaronColacoAsp.NETProject.Models
{
    public class GiftRecipient
    {
        [Key]
        public int RecipientId { get; set; }
        public int Name { get; set; }
        public string PhoneNumber { get; set; }

        public  string Address { get; set; }

        public string City { get; set; }

       
    }
}
