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
            return View(); /// show own Add view
        }

        [HttpPost]
        public async Task<IActionResult> Add(Item item) /// add product
        {
            if (!ModelState.IsValid)
            {
                return View(item); /// show errors
            }

            _dbContext.Items.Add(item);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ViewAll", "Item");
        }

        public async Task<IActionResult> Delete(int id) /// show form only
        {

            var item = await _dbContext.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return RedirectToAction("ViewAll", "Item");
            }

             _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ViewAll", "Item");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) /// show form only with id passed through route
        {
            var item = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == id);

            return (item is not null) ? View(item) : RedirectToAction("ViewAll", "Item");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Item item) 
        {
            if (!ModelState.IsValid)
            {
                return View(); 
            }

            var it= await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == item.Id);

            if (it == null)
            {
                return RedirectToAction("ViewAll", "Item");
            }

            it.PricePerItem=item.PricePerItem;
            it.Description=item.Description??null;
            it.Name=item.Name;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ViewAll", "Item");
        }
    }
}
