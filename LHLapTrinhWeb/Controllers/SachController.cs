using LHLapTrinhWeb.Models;
using LHLapTrinhWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class SachController : Controller
{
    private readonly DataContext _dataContext;

    public SachController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IActionResult BookList()
    {
            var sach = _dataContext.Saches
                .OrderByDescending(s => s.SoLuongBan)
                .ToList();

            return View(sach);
    }
}
