using System.ComponentModel.DataAnnotations;

namespace AaronColacoAsp.NETProject.Models
{

        public class Items
        {
           [Key]
            public int ItemId { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public decimal CostToProduce { get; set; }
            public string ImageURL { get; set; }

        }

 
}

