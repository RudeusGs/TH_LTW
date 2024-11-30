using LHLapTrinhWeb.Models;
using LHLapTrinhWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LHLapTrinhWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataContext _dataContext;

        public AdminController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult FormLogin()
        {
            ViewData["IsLoggedIn"] = HttpContext.Session.GetString("UserName") != null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin model)
        {
            if (ModelState.IsValid)
            {
                var Admin = await _dataContext.Admins
                    .FirstOrDefaultAsync(kh => kh.TenDnadmin == model.TenDnadmin && kh.MatKhauAdmin == model.MatKhauAdmin);

                if (Admin != null)
                {
                    HttpContext.Session.SetString("UserName", Admin.TenDnadmin);
                    HttpContext.Session.SetString("UserRole", "admin");
                    return RedirectToAction("BookList", "Sach");
                }
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            ViewData["IsLoggedIn"] = false;
            return View("FormLogin", model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserRole");
            return RedirectToAction("FormLogin");
        }
    }
}
