using System.ComponentModel.DataAnnotations;

namespace AaronColacoAsp.NETProject.Models
{
    public class Gift
    {
        [Key]
        public int GiftId { get; set; }

        public string Message { get; set; }

        public string RibbonColour { get; set; }

        public string BoxColour { get; set; }

       




    }
}
