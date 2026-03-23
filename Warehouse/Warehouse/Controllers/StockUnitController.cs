using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Warehouse.Data.DTOs.ItemDTOs;
using Warehouse.Data.DTOs.StockUnitDTOs;
using Warehouse.Services;

namespace Warehouse.Controllers
{
    public class StockUnitController : Controller
    {
        private readonly StockUnitService _service;

        private static int ItemId {  get; set; }
        public StockUnitController(StockUnitService _service)
        {
            this._service = _service;
        }


        [HttpGet]
        public async Task<IActionResult> ViewAll() /// view absolute all stock items
        {
            var items = await _service.getAll();
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllSelectedProducts(int id) /// view a subrage of stock items , dictated by item type
        {
            ItemId = id;

            var items = await _service.searchStockItemByItemId(id);

            if (items is null)
            {
                TempData["Error"] = "Nothing found";
                return RedirectToAction("ViewAll", "Item");
            }

            return RedirectToAction("ViewAll", "StockUnit");
        }


        [HttpGet] /// filter by name
        public async Task<IActionResult> SearchByName(string name)
        {
            var items = await _service.searchStockItemByItemName(name);

            if (items is null)
            {
                TempData["Error"] = "Nothing found";
                return RedirectToAction("ViewAll", "StockUnit");
            }

            return View("ViewAll", items);
        }

        [HttpGet] /// filter by barcode
        public async Task<IActionResult> SearchByBarcode(string barcode)
        {
            var items = await _service.searchStockItemByBarcode(barcode);

            if (items is null)
            {
                TempData["Error"] = "Nothing found";
                return RedirectToAction("ViewAll", "StockUnit");
            }

            return View("ViewAll", items);
        }

        

        [HttpGet]
        public IActionResult Add()
        {
            return View(); 
        }

        [HttpPost] 
        public async Task<IActionResult> Add(StockUnitCreateDTO item)
        {
            if (!ModelState.IsValid)
            {
                return View(item); 
            }

            item.ItemId = ItemId;

            var result = await _service.add(item);

            if (result is null)
            {
                TempData["Error"] = "Error adding";
                RedirectToAction("ViewAll","StockUnit",ItemId);
            }

            return RedirectToAction("ViewAll", "StockUnit", ItemId);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.delete(id);

            if (!ok)
                TempData["Error"] = "Failed to delete item.";
            else
                TempData["Success"] = "Item deleted successfully.";

            return RedirectToAction("ViewAll", "StockUnit");
        }

        
        [HttpGet] /// get selected item and pass it to edit
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.findById(id);
            if (result is null)
            {
                TempData["Error"] = "Error editing";
                return RedirectToAction("ViewAll", "StockUnit");
            }
            return View(result);
        }
         
         
        [HttpPost] /// update item with new data from form
        public async Task<IActionResult> Edit(int id, StockUnitUpdateDTO item)
        {
            if (!ModelState.IsValid)
            {
                return View(await _service.findById(id));
            }

            var it = await _service.edit(id, item);

            if (it is null)
                TempData["Error"] = "Failed to edit item.";
            else
                TempData["Success"] = "Item edited successfully.";

            return RedirectToAction("ViewAll", "StockUnit");
        }
    }
}
