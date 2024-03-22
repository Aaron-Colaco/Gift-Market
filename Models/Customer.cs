using Microsoft.AspNetCore.Identity;

namespace AaronColacoAsp.NETProject.Models
{
    public class Customer: IdentityUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public int PhoneNumebr { get; set; }

        public List <Order> Orders{ get; set; }
    }
}
