using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{
    public class GiftRecipient
    {
        [Key]
        public string RecipientId { get; set; }


        [Required,MaxLength(50)]
        public string Name { get; set; }

        // Phone number feild, validated with a regular expression
        [RegularExpression(@"^\+?\d{1,3}[- ]?\(?\d{3}\)?[- ]?\d{3}[- ]?\d{4}$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        //sets up one to one relastionship with gift
        [ForeignKey("Gift")]
        public string GiftId { get; set; }
        public Gift Gift { get; set; }
       


    }
}
