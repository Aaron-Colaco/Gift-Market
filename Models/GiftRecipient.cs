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

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

 
        [ForeignKey("Gift")]
        public string GiftId { get; set; }
        public Gift Gift { get; set; }
       


    }
}
