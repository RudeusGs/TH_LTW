using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LHLapTrinhWeb.Repository.Components
{
	public class ChudeViewComponent : ViewComponent
	{
		private readonly DataContext _dataContext;

		public ChudeViewComponent(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var chudes = await _dataContext.Chudes.ToListAsync();
            return View(chudes);
        }
    }
}
