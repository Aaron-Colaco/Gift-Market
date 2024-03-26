using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AaronColacoAsp.NETProject.Models
{
    public class Customer: IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(100)]
        public string DeliveryAddress { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; }

        [Required, DataType(DataType.PostalCode)]
        public int PostalCode { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int PhoneNumebr { get; set; }

        public List <Order> Orders{ get; set; }
    }
}
