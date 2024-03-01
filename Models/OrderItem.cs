
using System;

public class OrderItem
{
	public OrderItem()
	{
	   [key]
	   public int OrderItemId { get; set; }
	   public int Quantity { get; set; }

	   [ForeignKey("Order")]
       public int  OrderId { get; set; } { get; set; }
       public Orders Orders { get; set; }


       [ForeignKey("Item")]
       public int ItemId { get; set; }
       public Item Items { get; set; }


{ 
}
