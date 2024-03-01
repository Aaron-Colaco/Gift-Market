using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{
    public class OrderNote
    {
        [Key]
        public int OrderNoteId { get; set; }

        public string Note { get; set; }


        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public Order Orders { get; set; }

    }
}
