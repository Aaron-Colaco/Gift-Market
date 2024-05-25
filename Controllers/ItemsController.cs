using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AaronColacoAsp.NETProject.Data;
using AaronColacoAsp.NETProject.Models;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace AaronColacoAsp.NETProject.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }


        
        public async Task<IActionResult> Search(string SearchTerm)
        {
            var Results = _context.Item.Where(i => i.Name.Contains(SearchTerm)).Include(i => i.Categorys);
            return View("Index", await Results.ToListAsync());
        }



         public async Task<IActionResult> FilterByCategroy(int id)
        {
            var Results = _context.Item.Where(i => i.CategoryId == id).Include(i => i.Categorys);
            return View("Index", await Results.ToListAsync());
        }

        public async Task<IActionResult> Filter(int MinPrice, int MaxPrice)
        {
            var Results = _context.Item.Where(i => i.Price >= MinPrice && i.Price <= MaxPrice).Include(i => i.Categorys);

            return View("Index", await Results.ToListAsync());
        }





        public async Task<IActionResult> Index(int page = 1,int Item =1,bool displayPopUp = false)
        {
            ViewBag.Item = _context.Item.Where(a => a.ItemId == Item).FirstOrDefault();
            ViewBag.Dp = displayPopUp;
            var Items = _context.Item.Include(i => i.Categorys);

            const int ItemsPerPage = 6;
            ViewBag.Pages = (int)Math.Ceiling((double)Items.Count()/ItemsPerPage) ;
            return View(await Items.Skip((page-1)*ItemsPerPage).Take(6).ToListAsync());

        }


        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Categorys)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }



        [Authorize(Roles = "Admin")]
       
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ItemId,Name,Price,CostToProduce,ImageURL,Description,CategoryId")] Item item)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", item.Categorys.Name);
            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Name");
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Name,Price,CostToProduce,ImageURL,Description,CategoryId")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryList"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", item.Categorys.Name);
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Categorys)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item != null)
            {
                _context.Item.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }
    }
}
