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

    public async Task<IActionResult> BookList(bool isTopicSelected, int? MaCd = null, int? MaNxb = null)
    {
        IQueryable<Sach> query = _dataContext.Saches.Include(s => s.MaCdNavigation).Include(s => s.MaNxbNavigation);
        ViewBag.HasSelectedTopic = isTopicSelected;
        if (MaCd.HasValue)
        {
            query = query.Where(s => s.MaCd == MaCd.Value);
        }

        if (MaNxb.HasValue)
        {
            query = query.Where(s => s.MaNxb == MaNxb.Value);
        }

        var books = await query.ToListAsync();
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
}
