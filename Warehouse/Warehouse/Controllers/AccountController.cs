using Microsoft.AspNetCore.Mvc;
using Warehouse.Data.DTOs.AccountDTOs;
using Warehouse.Data.Entities;
using Warehouse.Services;

namespace Warehouse.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login() => View();
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO data)
        {
            if (!ModelState.IsValid)
                return View();

            var (ok,msg,view) = await _service.login(data);

            if (!ok)
            {
                TempData["Error"] = msg;
                return View();
            }

            /// values of the session
            HttpContext.Session.SetInt32("UserId", view.Id);
            HttpContext.Session.SetString("UserEmail", view.Email);
            HttpContext.Session.SetString("AccountType", view.AccountType.ToString());

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult RegisterCustomer() => View();
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(AccountCreateDTO data)
        {
            if (!ModelState.IsValid)
                return View(data);

            data.AccountType = AccountType.Customer;

            var (success, error, _) = await _service.register(data);

            if (!success)
            {
                TempData["Error"] = error;
                return View(data);
            }            

            TempData["Success"] = "Account created.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult RegisterStaff() => View();
        [HttpPost]
        public async Task<IActionResult> RegisterStaff(AccountCreateDTO data)
        {
            if (!ModelState.IsValid)
                return View(data);

            // Only Seller or Admin allowed here
            if (data.AccountType == AccountType.Customer)
            {
                TempData["Error"] = "Invalid account type for staff registration.";
                return View(data);
            }

            var (success, error, _) = await _service.register(data);

            if (!success)
            {
                TempData["Error"] = error;
                return View(data);
            }

            TempData["Success"] = "Staff account created. Please log in.";
            return RedirectToAction("Index", "Home");
        }

    }
}