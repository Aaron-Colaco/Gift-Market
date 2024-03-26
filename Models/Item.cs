using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaronColacoAsp.NETProject.Models
{

        public class Item
        {
           [Key]
            public int ItemId { get; set; }
            
            [Required, MaxLength (50)]
            public string Name { get; set; }


            [Required,DataType(DataType.Currency),Range(0,600)]
            public decimal Price { get; set; }


            [Required,DataType(DataType.Currency),Range(0,500)]
            public decimal CostToProduce { get; set; }

            [MaxLength(1000),DataType(DataType.Url)]
            public string ImageURL { get; set; }
           
            [MaxLength(100)]
            public string Description { get; set; }

            [ForeignKey("Category"),Required]
            public int CategoryId { get; set; }
            public Category Categorys { get; set; }

        [Range(1,100)]
        public int stockLevel { get; set; } = 0;




    }

 
}

