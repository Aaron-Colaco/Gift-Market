using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{

        public class Item
        {
           [Key]
            public int ItemId { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public decimal CostToProduce { get; set; }
            public string ImageURL { get; set; }

           [ForeignKey("Category")]
            public int CategoryId { get; set; }
            public Category Categorys { get; set; }
            



    }

 
}

