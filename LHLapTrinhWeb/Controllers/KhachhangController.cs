using LHLapTrinhWeb.Models;
using LHLapTrinhWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LHLapTrinhWeb.Controllers
{
    public class KhachhangController : Controller
    {
        private readonly DataContext _dataContext;

        public KhachhangController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult FormLogin()
        {
            ViewData["IsLoggedIn"] = HttpContext.Session.GetString("UserName") != null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Khachhang model)
        {
            if (ModelState.IsValid)
            {
                var customer = await _dataContext.Khachhangs
                    .FirstOrDefaultAsync(kh => kh.TenDn == model.TenDn && kh.MatKhau == model.MatKhau);

                if (customer != null)
                {
                    HttpContext.Session.SetString("UserName", customer.TenDn);
                    return RedirectToAction("BookList", "Sach");
                }
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            ViewData["IsLoggedIn"] = false;
            return View("FormLogin", model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("FormLogin", "Khachhang");
        }
        public async Task<IActionResult> Profile()
        {
            var username = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("FormLogin");
            }

            var customer = await _dataContext.Khachhangs
                .FirstOrDefaultAsync(kh => kh.TenDn == username);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Khachhang model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _dataContext.Khachhangs
                    .FirstOrDefaultAsync(kh => kh.TenDn == model.TenDn);

                if (existingUser != null)
                {
                    ModelState.AddModelError("TenDn", "Tên đăng nhập đã tồn tại.");
                    return View(model);
                }
                await _dataContext.Khachhangs.AddAsync(model);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction("FormLogin", "Khachhang");
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var khachHang = GetCurrentCustomer();

            if (currentPassword == null || newPassword == null || confirmPassword == null)
            {
                ModelState.AddModelError("", "All fields are required.");
                return View("Profile", khachHang);
            }

            if (!ValidateCurrentPassword(khachHang, currentPassword))
            {
                ModelState.AddModelError("currentPassword", "Current password is incorrect.");
                return View("Profile", khachHang);
            }
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "New password and confirmation do not match.");
                return View("Profile", khachHang);
            }
            _dataContext.Update(khachHang);
            await _dataContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Password changed successfully.";
            return RedirectToAction("Profile");
        }

        private Khachhang GetCurrentCustomer()
        {
            return new Khachhang(); 
        }
        private bool ValidateCurrentPassword(Khachhang khachHang, string currentPassword)
        {
            return true; 
        }

    }
}
