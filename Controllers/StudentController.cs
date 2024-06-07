using Microsoft.AspNetCore.Mvc;
using dms.Models;
using System.Linq;
using System.Security.Claims;

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

        public IActionResult Repair()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitRepair(string address, string description, string category)
        {
            var studentName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value; // 获取当前登录用户的姓名
            var studentNo = User.Claims.FirstOrDefault(c => c.Type == "StudentNo")?.Value; // 获取当前登录用户的学号

            var repair = new Repair
            {
                StudentName = studentName,
                StudentNo = studentNo,
                Address = address,
                Description = description,
                Category = category,
                SubmitTime = DateTime.Now,
                Status = "未开始"
            };

            _context.Repairs.Add(repair);
            _context.SaveChanges();

            ViewBag.Message = "报修提交成功！";
            return View("RepairConfirmation");
        }
        public IActionResult RepairHistory(int pageIndex = 0)
        {
            var studentNo = User.Claims.FirstOrDefault(c => c.Type == "StudentNo")?.Value;
            if (string.IsNullOrEmpty(studentNo))
            {
                return RedirectToAction("Login", "Auth");
            }

            const int PageSize = 10;
            var totalRepairs = _context.Repairs.Count(r => r.StudentNo == studentNo);
            var repairs = _context.Repairs
                .Where(r => r.StudentNo == studentNo)
                .OrderByDescending(r => r.SubmitTime)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalRepairs;
            ViewBag.PageIndex = pageIndex;

            return View(repairs);
        }
    }
}
