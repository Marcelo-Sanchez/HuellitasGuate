using HuellitasGuate.Data;
using HuellitasGuate.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuellitasGuate.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HuellitasGuateContext _context;

        public DashboardController(HuellitasGuateContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hoy = DateTime.Today;
            var dashboardStats = new DashboardStats

            {
                TotalCitas = _context.Citas.Count(),
                //CitasHoy = _context.Citas.Count(c => c.Fecha == DateTime.Today)
                CitasHoy = _context.Citas.Count(c => c.Fecha.HasValue && c.Fecha.Value.Date == hoy.Date)

        };

            return View(dashboardStats);
        }
    }

}
