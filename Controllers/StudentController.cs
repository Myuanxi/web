using Microsoft.AspNetCore.Mvc;
using dms.Models;
using System.Linq;

namespace dms.Controllers
{
    public class StudentController : Controller
    {
        private readonly DmsContext _context;
        private const int PageSize = 10; // 每页显示的通知数

        public StudentController(DmsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Notifications(int pageIndex = 0)
        {
            var totalNotifications = _context.Notifications.Count();
            var notifications = _context.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalNotifications;
            ViewBag.PageIndex = pageIndex;

            return View(notifications);
        }
    }
}
