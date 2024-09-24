using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LHLapTrinhWeb.Repository.Components
{
    public class NhaxuatbanViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext;

        public NhaxuatbanViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var nhaxuatbans = await _dataContext.Nhaxuatbans.ToListAsync();
            return View(nhaxuatbans);
        }
    }
}
