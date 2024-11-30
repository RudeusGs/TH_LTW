using LHLapTrinhWeb.Filters;
using LHLapTrinhWeb.Models;
using LHLapTrinhWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

public class SachController : Controller
{
    private readonly DataContext _dataContext;
    private const string CartSessionKey = "CartSession";

    public SachController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IActionResult> BookList(bool isTopicSelected, int? MaCd = null, int? MaNxb = null, int page = 1)
    {
        int pageSize = 12;

        IQueryable<Sach> query = _dataContext.Saches
            .Include(s => s.MaCdNavigation)
            .Include(s => s.MaNxbNavigation);

        ViewBag.HasSelectedTopic = isTopicSelected;
        if (MaCd.HasValue)
        {
            query = query.Where(s => s.MaCd == MaCd.Value);
        }

        if (MaNxb.HasValue)
        {
            query = query.Where(s => s.MaNxb == MaNxb.Value);
        }
        var totalBooks = await query.CountAsync();
        var books = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(books);
    }


    public IActionResult Detail(int id)
    {
        var sach = _dataContext.Saches.Find(id);
        if (sach == null)
        {
            return NotFound();
        }
        return View(sach);
    }

    public async Task<IActionResult> ShoppingCart()
    {
        var cart = GetCartItems();
        var cartItems = new List<Sach>();

        foreach (var item in cart)
        {
            var sach = await _dataContext.Saches.FindAsync(item.Key);
            if (sach != null)
            {
                sach.SoLuongBan = item.Value;
                cartItems.Add(sach);
            }
        }

        ViewBag.Subtotal = cartItems.Sum(s => (s.DonGia ?? 0) * s.SoLuongBan);
        ViewBag.DeliveryCharge = 100;
        ViewBag.Total = ViewBag.Subtotal + ViewBag.DeliveryCharge;

        return View(cartItems);
    }

    private Khachhang GetCurrentCustomer()
    {
        var username = HttpContext.Session.GetString("UserName");
        return _dataContext.Khachhangs.FirstOrDefault(kh => kh.TenDn == username);
    }

    [HttpPost]
    public IActionResult AddToCart(int id)
    {
        var username = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(username))
        {
            TempData["ErrorMessage"] = "Bạn cần đăng nhập để thêm vào giỏ hàng.";
            return RedirectToAction("FormLogin", "Khachhang");
        }

        var sach = _dataContext.Saches.Find(id);
        if (sach == null)
        {
            return NotFound();
        }

        var cart = GetCartItems();
        if (cart.ContainsKey(sach.MaSach))
        {
            cart[sach.MaSach]++;
        }
        else
        {
            cart[sach.MaSach] = 1;
        }

        SaveCartSession(cart);
        return RedirectToAction("ShoppingCart");
    }

    public async Task<IActionResult> OrderDetails(int id)
    {
        var order = await _dataContext.Dondathangs
            .Include(o => o.Ctdathangs)
                .ThenInclude(c => c.MaSachNavigation)
            .Include(o => o.MaKhNavigation)
            .FirstOrDefaultAsync(o => o.SoDh == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPost]
    public IActionResult UpdateCart(int id, int quantity)
    {
        var cart = GetCartItems();

        if (cart.ContainsKey(id))
        {
            if (quantity > 0)
            {
                cart[id] = quantity;
            }
            else
            {
                cart.Remove(id);
            }
            SaveCartSession(cart);
        }

        return RedirectToAction("ShoppingCart");
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int id)
    {
        var cart = GetCartItems();

        if (cart.ContainsKey(id))
        {
            cart.Remove(id);
            SaveCartSession(cart);
        }

        return RedirectToAction("ShoppingCart");
    }

    private Dictionary<int, int> GetCartItems()
    {
        var session = HttpContext.Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(session))
        {
            return new Dictionary<int, int>();
        }
        return JsonConvert.DeserializeObject<Dictionary<int, int>>(session);
    }

    private void SaveCartSession(Dictionary<int, int> cart)
    {
        var session = JsonConvert.SerializeObject(cart);
        HttpContext.Session.SetString(CartSessionKey, session);
    }

    public async Task<IActionResult> Checkout()
    {
        var username = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(username))
        {
            TempData["ErrorMessage"] = "Bạn cần đăng nhập để đặt hàng.";
            return RedirectToAction("FormLogin", "Khachhang");
        }

        var cart = GetCartItems();
        if (cart == null || !cart.Any())
        {
            TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
            return RedirectToAction("ShoppingCart");
        }

        var khachHang = GetCurrentCustomer();
        if (khachHang == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy khách hàng.";
            return RedirectToAction("ShoppingCart");
        }

        var newOrder = new Dondathang
        {
            MaKh = khachHang.MaKh,
            NgayDh = DateTime.Now,
            TriGia = cart.Sum(item => _dataContext.Saches.Find(item.Key).DonGia * item.Value),
            NgayGiaoHang = DateTime.Now.AddDays(3),
            TenNguoiNhan = khachHang.HoTenKh,
            DiaChiNhan = khachHang.DiaChiKh,
            DienThoaiNhan = khachHang.DienThoaiKh,
            DaGiao = false,
            HtthanhToan = false,
            HtgiaoHang = false
        };

        _dataContext.Dondathangs.Add(newOrder);
        await _dataContext.SaveChangesAsync();

        foreach (var item in cart)
        {
            var sach = await _dataContext.Saches.FindAsync(item.Key);
            if (sach != null)
            {
                var orderDetail = new Ctdathang
                {
                    SoDh = newOrder.SoDh,
                    MaSach = sach.MaSach,
                    SoLuong = item.Value,
                    DonGia = sach.DonGia,
                    ThanhTien = sach.DonGia * item.Value
                };
                _dataContext.Ctdathangs.Add(orderDetail);
            }
        }

        await _dataContext.SaveChangesAsync();

        HttpContext.Session.Remove(CartSessionKey);
        TempData["SuccessMessage"] = "Đặt hàng thành công! Đơn hàng của bạn sẽ được xử lý trong thời gian sớm nhất.";
        return RedirectToAction("OrderDetails", new { id = newOrder.SoDh });
    }
    [AuthorizeRole("admin")]
    public async Task<IActionResult> Index(int page = 1)
    {
        int pageSize = 20;

        IQueryable<Sach> query = _dataContext.Saches
            .Include(s => s.MaCdNavigation)
            .Include(s => s.MaNxbNavigation);
        var totalBooks = await query.CountAsync();
        var books = await query
            .Skip((page - 1) * pageSize) 
            .Take(pageSize) 
            .ToListAsync();
        var totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(books);
    }

    [AuthorizeRole("admin")]
    public IActionResult Create()
    {
        ViewBag.ChuDes = new SelectList(_dataContext.Chudes, "MaCd", "TenChuDe");
        ViewBag.NhaXuatBans = new SelectList(_dataContext.Nhaxuatbans, "MaNxb", "TenNxb");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthorizeRole("admin")]
    public async Task<IActionResult> Create(Sach sach, IFormFile hinhMinhHoa)
    {
        if (hinhMinhHoa != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(hinhMinhHoa.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("HinhMinhHoa", "Chỉ cho phép tải lên các tệp hình ảnh (.jpg, .jpeg, .png).");
                return View(sach);
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", hinhMinhHoa.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await hinhMinhHoa.CopyToAsync(stream);
            }
            sach.HinhMinhHoa = "/images/" + hinhMinhHoa.FileName;
        }

        if (ModelState.IsValid)
        {
            _dataContext.Saches.Add(sach);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(sach);
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();
        var sach = await _dataContext.Saches.FindAsync(id);
        if (sach == null)
            return NotFound();

        ViewBag.ChuDes = new SelectList(_dataContext.Chudes, "MaCd", "TenChuDe", sach.MaCd);
        ViewBag.NhaXuatBans = new SelectList(_dataContext.Nhaxuatbans, "MaNxb", "TenNxb", sach.MaNxb);

        return View(sach);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthorizeRole("admin")]
    public async Task<IActionResult> Edit(int id, Sach sach, IFormFile HinhMinhHoa)
    {
        if (id != sach.MaSach)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var existingSach = await _dataContext.Saches.FindAsync(id);
                if (existingSach == null)
                    return NotFound();
                existingSach.TenSach = sach.TenSach;
                existingSach.DonViTinh = sach.DonViTinh;
                existingSach.DonGia = sach.DonGia;
                existingSach.MoTa = sach.MoTa;
                existingSach.MaCd = sach.MaCd;
                existingSach.MaNxb = sach.MaNxb;
                if (HinhMinhHoa != null && HinhMinhHoa.Length > 0)
                {
                    var fileName = Path.GetFileName(HinhMinhHoa.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/Sach", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhMinhHoa.CopyToAsync(stream);
                    }

                    existingSach.HinhMinhHoa = fileName;
                }

                _dataContext.Update(existingSach);
                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SachExists(sach.MaSach))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewBag.ChuDes = new SelectList(_dataContext.Chudes, "MaCd", "TenChuDe", sach.MaCd);
        ViewBag.NhaXuatBans = new SelectList(_dataContext.Nhaxuatbans, "MaNxb", "TenNxb", sach.MaNxb);

        return View(sach);
    }
    [AuthorizeRole("admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var sach = await _dataContext.Saches
            .Include(s => s.MaCdNavigation)
            .Include(s => s.MaNxbNavigation)
            .FirstOrDefaultAsync(m => m.MaSach == id);
        if (sach == null)
            return NotFound();

        return View(sach);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AuthorizeRole("admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var sach = await _dataContext.Saches
                .Include(s => s.Ctdathangs)
                .Include(s => s.Vietsaches)
                .FirstOrDefaultAsync(s => s.MaSach == id);

            if (sach == null)
                return NotFound();
            if (sach.Ctdathangs.Any())
            {
                _dataContext.Ctdathangs.RemoveRange(sach.Ctdathangs);
            }
            if (sach.Vietsaches.Any())
            {
                _dataContext.Vietsaches.RemoveRange(sach.Vietsaches);
            }
            _dataContext.Saches.Remove(sach);
            await _dataContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa sách thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa sách. Vui lòng thử lại sau.";
            return RedirectToAction("Delete", new { id });
        }
    }

    private bool SachExists(int id)
    {
        return _dataContext.Saches.Any(e => e.MaSach == id);
    }
}
