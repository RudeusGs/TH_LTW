using LHLapTrinhWeb.Models;
using LHLapTrinhWeb.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LHLapTrinhWeb.Controllers
{
    public class KhachhangController : Controller
    {
        private readonly DataContext _dataContext;
        public KhachhangController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Khachhang model)
        {
            if (ModelState.IsValid)
            {
                var customer = _dataContext.Khachhangs
                    .FirstOrDefault(kh => kh.TenDn == model.TenDn && kh.MatKhau == model.MatKhau);

                if (customer != null)
                {
                    return RedirectToAction("BookList", "Sach");
                }
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            return View(model);
        }
    }
}
