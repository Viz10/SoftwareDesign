using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.DbRepository;
using Warehouse.Data.DTOs;
using Warehouse.Data.Entities;
using Warehouse.Services;

namespace Warehouse.Controllers
{
    public class ItemController : Controller
    {
        private readonly IServiceItem _service;
        public ItemController(IServiceItem _service)  
        {
            this._service = _service;
        }


        [HttpGet]
        public async Task<IActionResult> ViewAll()
        {
            var items = await _service.getAllItems();
            return View(items);
        }


        [HttpGet]
        public IActionResult Add() /// show form only
        {
            return View(); /// show own Add view
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Add(ItemDTO item) /// add product
        {
            if (!ModelState.IsValid)
            {
                return View(item); /// show errors
            }

            await _service.addItem(item);

            return RedirectToAction("ViewAll", "Item");
        }

        
        [HttpGet] // GET /Item/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.findById(id);
            if (!result.Item2)
            {
                TempData["Error"] = "Error editing";
                return RedirectToAction("ViewAll", "Item");
            }
            ViewBag.ItemId = id;
            return View(result.Item1);
        }

        
        [HttpPost]  // POST /Item/Edit/5
        public async Task<IActionResult> Edit(int id, ItemDTO item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ItemId = id;
                return View(item);
            }

            var it = await _service.editItem(id, item);

            if (it is null)
                TempData["Error"] = "Failed to edit item.";
            else
                TempData["Success"] = "Item edited successfully.";

            return RedirectToAction("ViewAll", "Item");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.deleteItem(id);

            if (!ok)
                TempData["Error"] = "Failed to delete item.";
            else
                TempData["Success"] = "Item deleted successfully.";

            return RedirectToAction("ViewAll", "Item");
        }
    }

}
