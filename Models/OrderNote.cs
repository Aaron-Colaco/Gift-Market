using System.ComponentModel.DataAnnotations;

namespace AaronColacoAsp.NETProject.Models
{
    public class OrderNote
    {
        [Key]
        public int OrderNoteId { get; set; }

        public string Note { get; set; }
    }
}
