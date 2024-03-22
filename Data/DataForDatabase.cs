using AaronColacoAsp.NETProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace AaronColacoAsp.NETProject.Data
{

    public class DataForDatabase
    {
        public static void Initialize(ApplicationDbContext Context)
        {
            Context.Database.EnsureCreated();

            //check if any items already exist
            if (Context.Item.Any())
            {
                //database has data 
                return;
            }
            

            var Categorys = new Category[]
            {
                new Category { Name = "Food" },
                new Category { Name = "Drinks" },
                new Category { Name = "Physical Products" },
                new Category { Name = "Pre-Packed" },


            };

            var Items = new Item[]
            {
                new Item{Name = "Whittaker's Creamny Milk",CategoryId=1,ImageURL="",CostToProduce=5, Price= 7},
                new Item{Name = "Whittaker's Peanut Slab",CategoryId=1,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "Pineapple Lumps",CategoryId=1,ImageURL="",CostToProduce=3, Price=4 },
                new Item{Name = "Original Cookie Time Pack",CategoryId=1,ImageURL="",CostToProduce=5, Price=8 },
                new Item{Name = "Delsio Chips",CategoryId=1,ImageURL="",CostToProduce=4, Price=6 },
                new Item{Name = "Manuka Honey",CategoryId=1,ImageURL="",CostToProduce=75, Price=150 },
                new Item{Name = "Kumara Chips",CategoryId=1,ImageURL="",CostToProduce=4, Price=6 },


                new Item{Name = "L & P Orignal",CategoryId=2,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "RARO Navel Orange",CategoryId=2,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "V Refresh Green apple",CategoryId=2,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "V Refresh Pineapple and watermelon ",CategoryId=2,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "VIBE Grape sparkilng Water",CategoryId=2,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "JEDS Coffee",CategoryId=2,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "Just Jucie ",CategoryId=2,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "V Refresh Green apple Pack",CategoryId=2,ImageURL="",CostToProduce=2, Price=4 },


                new Item{Name = "Prezzy Card $100",CategoryId=3,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "Prezzy Card $50",CategoryId=3,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "Prezzy Card $25",CategoryId=3,ImageURL="",CostToProduce=2, Price=4 },
                new Item{Name = "Delsis",CategoryId=3,ImageURL="",CostToProduce=4, Price=6 },
                new Item{Name = "Manuka Honey Soap",CategoryId=3,ImageURL="",CostToProduce=4, Price=6 },
                new Item{Name = "LemonGrass Soap",CategoryId=3,ImageURL="",CostToProduce=4, Price=6 },
                new Item{Name = "All Blacks Shirt",CategoryId=3,ImageURL="",CostToProduce=4, Price=6 },
                new Item{Name = "Black Caps Shirt",CategoryId=3,ImageURL="",CostToProduce=4, Price=6 },
                new Item{Name = "New Zealand flag",CategoryId=1,ImageURL="",CostToProduce=4, Price=6 }
            };

            Context.Category.AddRange(Categorys);
            Context.Item.AddRange(Items);
            

















            }






        }


    }


