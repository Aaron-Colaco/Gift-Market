using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AaronColacoAsp.NETProject.Models
{
    public class Customer: IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; }


        [RegularExpression(@"^\+?\d{1,3}[- ]?\(?\d{3}\)?[- ]?\d{3}[- ]?\d{4}$", ErrorMessage = "Invalid phone number format")]
        public int PhoneNumebr { get; set; }

        public List <Order> Orders{ get; set; }
    }
}
