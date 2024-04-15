
using AaronColacoAsp.NETProject.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace AaronColacoAsp.NETProject.Models
{

    public class OrderItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderItemId { get; set; }

        [Range(0,10),Required]
                
        public int Quantity { get; set; } = 1;

        [ForeignKey("Order")]
        public string OrderId { get; set; }
        public Order Orders { get; set; }


        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Items { get; set; }



    }
}