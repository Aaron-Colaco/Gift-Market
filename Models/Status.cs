using System.ComponentModel.DataAnnotations;

namespace AaronColacoAsp.NETProject.Models
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        public string Name { get; set; }

    }
}
