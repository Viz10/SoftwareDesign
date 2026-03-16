using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.DbRepository;
using Warehouse.Data.Entities;

namespace Warehouse.Controllers
{
    public class ItemController(WarehouseDbContext DbContext) : Controller
    {
        private WarehouseDbContext _dbContext { get; } = DbContext;

        [HttpGet]
        public async Task<IActionResult> ViewAll()
        {
            var priductItems = await _dbContext.Items.ToListAsync();
            return View(priductItems);
        }
        
        [HttpGet]
        public IActionResult Add() /// show form only
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Item item) /// add product
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            _dbContext.Items.Add(item);
            _dbContext.SaveChanges();
            return RedirectToAction("ViewAll", "Item");
        }
    }
}
