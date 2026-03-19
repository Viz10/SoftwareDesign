using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.DbRepository;
using Warehouse.Data.DTOs.ItemDTOs;
using Warehouse.Data.Entities;
using Warehouse.Services;

namespace Warehouse.Controllers
{
    public class ItemController : Controller
    {
        private readonly ItemService _service;
        public ItemController(ItemService _service)  
        {
            this._service = _service;
        }


        [HttpGet] /// return all product types
        public async Task<IActionResult> ViewAll()
        {
            var items = await _service.getAll();
            return View(items);
        }

        [HttpGet] /// show Add form only
        public IActionResult Add() 
        {
            return View(); /// show own Add view
        }

        [HttpGet] /// get selected item to edit
        public async Task<IActionResult> Edit(int id) 
        {
            var result = await _service.findById(id);
            if (result is null)
            {
                TempData["Error"] = "Error editing";
                return RedirectToAction("ViewAll", "Item");
            }
            ViewBag.ItemId = id;
            return View(result);
        }



        
        [HttpPost] /// add product with data from form
        public async Task<IActionResult> Add(ItemCreateDTO item) 
        {
            if (!ModelState.IsValid)
            {
                return View(item); /// show errors
            }

            var result = await _service.add(item);

            if(result is null)
            {
                TempData["Error"] = "Error adding";
                RedirectToAction("ViewAll", "Item");
            }

            return RedirectToAction("ViewAll", "Item");
        }

        [HttpPost] /// update item with new data from form
        public async Task<IActionResult> Edit(int id, ItemUpdateDTO item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ItemId = id;
                return View(item);
            }

            var it = await _service.edit(id, item);

            if (it is null)
                TempData["Error"] = "Failed to edit item.";
            else
                TempData["Success"] = "Item edited successfully.";

            return RedirectToAction("ViewAll", "Item");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.delete(id);

            if (!ok)
                TempData["Error"] = "Failed to delete item.";
            else
                TempData["Success"] = "Item deleted successfully.";

            return RedirectToAction("ViewAll", "Item");
        }
    }

}
