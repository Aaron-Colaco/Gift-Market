
using AaronColacoAsp.NETProject.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class OrderItem
{

	   [Key]
	   public string OrderItemId { get; set; }
	   public int Quantity { get; set; }

	   [ForeignKey("Order")]
       public string  OrderId { get; set; }
       public Order Orders { get; set; }


       [ForeignKey("Item")]
       public int ItemId { get; set; }
       public Item Items { get; set; }


 
}
