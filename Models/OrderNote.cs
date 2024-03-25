using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{
    public class OrderNote
    {
        [Key]
        public string OrderNoteId { get; set; }

        [Required]
        [MaxLength(120)]
        public string Note { get; set; }


        [ForeignKey("Order")]
        public string OrderId { get; set; }

        public Order Orders { get; set; }

    }
}
