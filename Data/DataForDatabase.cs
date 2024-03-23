using AaronColacoAsp.NETProject.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace AaronColacoAsp.NETProject.Data
{

    public class DataForDatabase
    {


        public static void AddData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var Context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                Context.Database.EnsureCreated();


                //check if any items already exist


                if (Context.Category.Any() || Context.Item.Any() || Context.Status.Any() || Context.Order.Any() || Context.OrderItem.Any() || Context.Gift.Any() || Context.GiftRecipient.Any() || Context.OrderNote.Any() )
                {
                    return;
                }

                else {

                    var Categorys = new Category[]
                {
                    new Category { Name = "Food"},
                    new Category { Name = "Drinks" },
                    new Category { Name = "Physical Products"},
                    new Category { Name = "Pre-Packed" },
                };
                    Context.Category.AddRange(Categorys);
                    Context.SaveChanges();


                  var ItemsData = new Item[]
                    {
                new Item{Name = "Whittaker's Creamny Milk",CategoryId=1,ImageURL="",CostToProduce=5, Price= 7 ,description ="highQualty"},
                new Item{Name = "Whittaker's Peanut Slab",CategoryId=1,ImageURL="",CostToProduce=2, Price=4,description ="highQualty" },
                new Item{Name = "Pineapple Lumps",CategoryId=1,ImageURL="",CostToProduce=3, Price=4,description ="highQualty" },
                new Item{Name = "Original Cookie Time Pack",CategoryId=1,ImageURL="",CostToProduce=5, Price=8,description ="highQualty" },
                new Item{Name = "Delsio Chips",CategoryId=1,ImageURL="",CostToProduce=4, Price=6 ,description ="highQualty"},
                new Item{Name = "Manuka Honey",CategoryId=1,ImageURL="",CostToProduce=75, Price=150 ,description ="highQualty"},
                new Item{Name = "Kumara Chips",CategoryId=1,ImageURL="",CostToProduce=4, Price=6,description ="highQualty" },


                new Item{Name = "L & P Orignal",CategoryId=2,ImageURL="",CostToProduce=2, Price=4, description ="highQualty"},
                new Item{Name = "RARO Navel Orange",CategoryId=2,ImageURL="",CostToProduce=2, Price=4,description ="highQualty" },
                new Item{Name = "V Refresh Green apple",CategoryId=2,ImageURL="",CostToProduce=2, Price=4 ,description ="highQualty"},
                new Item{Name = "V Refresh Pineapple and watermelon ",CategoryId=2,ImageURL="",CostToProduce=2, Price=4,description ="highQualty" },
                new Item{Name = "VIBE Grape sparkilng Water",CategoryId=2,ImageURL="",CostToProduce=2, Price=4,description ="highQualty" },
                new Item{Name = "JEDS Coffee",CategoryId=2,ImageURL="",CostToProduce=2, Price=4,description ="highQualty" },
                new Item{Name = "Just Jucie ",CategoryId=2,ImageURL="",CostToProduce=2, Price=4 ,description ="highQualty"},
                new Item{Name = "V Refresh Green apple Pack",CategoryId=2,ImageURL="",CostToProduce=2, Price=4,description ="highQualty" },


                new Item{Name = "Prezzy Card $100",CategoryId=3,ImageURL="",CostToProduce=2, Price=4,description ="highQualty"},
                new Item{Name = "Prezzy Card $50",CategoryId=3,ImageURL="",CostToProduce=2, Price=4 ,description ="highQualty"},
                new Item{Name = "Prezzy Card $25",CategoryId=3,ImageURL="",CostToProduce=2, Price=4 ,description ="highQualty"},
                new Item{Name = "Delsis",CategoryId=3,ImageURL="",CostToProduce=4, Price=6,description ="highQualty" },
                new Item{Name = "Manuka Honey Soap",CategoryId=3,ImageURL="",CostToProduce=4, Price=6,description ="highQualty" },
                new Item{Name = "LemonGrass Soap",CategoryId=3,ImageURL="",CostToProduce=4, Price=6 ,description ="highQualty"},
                new Item{Name = "All Blacks Shirt",CategoryId=3,ImageURL="",CostToProduce=4, Price=6,description ="highQualty" },
                new Item{Name = "Black Caps Shirt",CategoryId=3,ImageURL="",CostToProduce=4, Price=6,description ="highQualty" },
                new Item{Name = "New Zealand flag",CategoryId=1,ImageURL="",CostToProduce=4, Price=6,description ="highQualty" }
                    };






                    Context.Item.AddRange(ItemsData);

                    Context.SaveChanges();





                    var StatusData = new Status[]
                    {
                    new Status{Name = "Pending"},
                    new Status{Name = "Processing" }
                    };


                    Context.Status.AddRange(StatusData);

                    Context.SaveChanges();


                    var CustomerData = new Customer[]
                    {
                        new Customer{Id="58a",PasswordHash="#($**#*",UserName="Colacolaaron@gmail.com",FullName="Aaron Coaclo"}
                    };
              
                Context.Customer.AddRange(CustomerData);

                Context.SaveChanges();


                var OrderData = new Order[]
                    {
                    new Order{OrderTime = DateTime.Now, CustomerId="58a", StatusId=1,OrderId="1a",TotalPrice=0},
                    new Order{OrderTime = DateTime.Now, CustomerId="58a", StatusId=1,OrderId="2a",TotalPrice=0},
                    new Order{OrderTime = DateTime.Now, CustomerId="58a", StatusId=1,OrderId="3a",TotalPrice=0},
                    new Order{OrderTime = DateTime.Now, CustomerId="58a", StatusId=1,OrderId="4a",TotalPrice=0}
                    };

                    Context.Order.AddRange(OrderData);

                    Context.SaveChanges();







                    var OrderItemData = new OrderItem[]
                    {
                    new OrderItem{OrderId = "1a",ItemId=1,Quantity=2,OrderItemId="1a"},
                    new OrderItem{OrderId = "1a",ItemId=4,Quantity=1,OrderItemId="2a"},
                    new OrderItem{OrderId = "1a",ItemId=5,Quantity=3, OrderItemId="3a"},
                    new OrderItem{OrderId = "1a",ItemId=6,Quantity=5,OrderItemId="4a"},

                    new OrderItem{OrderId = "2a",ItemId=1,Quantity=2,OrderItemId="5a"},
                    new OrderItem{OrderId = "3a",ItemId=4,Quantity=1,OrderItemId="6a"},
                    new OrderItem{OrderId = "4a",ItemId=5,Quantity=3,OrderItemId="7a"},
                    new OrderItem{OrderId = "5a",ItemId=6,Quantity=5,OrderItemId="8a"},

                    new OrderItem{OrderId = "3a",ItemId=1,Quantity=2,OrderItemId="9a"},
                    new OrderItem{OrderId = "4a",ItemId=4,Quantity=1,OrderItemId="10a"},
                    new OrderItem{OrderId = "5a",ItemId=5,Quantity=3,OrderItemId="11a"},
                    new OrderItem{OrderId = "6a",ItemId=6,Quantity=5,OrderItemId="12a"},
                    };


                    Context.OrderItem.AddRange(OrderItemData);

                    Context.SaveChanges();






                    var GiftsData = new Gift[]
                    {
                    new Gift {OrderId ="1a",BoxColour="red",RibbonColour="purple",Message="thank you very Much",GiftId="gifttest"},
                    new Gift {OrderId ="2a",BoxColour="green",RibbonColour="purple",Message="thank you very Much Bob"},

                    };

                    Context.Gift.AddRange(GiftsData);

                    Context.SaveChanges();




                    var GiftReceiver = new GiftRecipient[]
                    {
                    new GiftRecipient{GiftId="gifttest",Address="123 road avondale",City="Auckland",Name="bob", PhoneNumber="0226765505"}
                    };


                    Context.GiftRecipient.AddRange(GiftReceiver);

                    Context.SaveChanges();


                }


            }



        }



    }

}










