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

        public IActionResult Chat()
        {
            var studentNo = User.Claims.FirstOrDefault(c => c.Type == "StudentNo")?.Value;
            if (string.IsNullOrEmpty(studentNo))
            {
                return RedirectToAction("Login", "Auth");
            }

            var student = _context.Students.FirstOrDefault(s => s.Sno == studentNo);
            if (student == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var dormitory = _context.Dormitories.FirstOrDefault(d => d.Id == student.DId);
            if (dormitory == null)
            {
                return RedirectToAction("Index");
            }

            var admin = _context.Admins.FirstOrDefault(a => a.Id == dormitory.AId);
            if (admin == null)
            {
                return RedirectToAction("Index");
            }

            var chatMessages = _context.ChatMessages
                .Where(cm => (cm.SenderId == student.Id && cm.ReceiverId == admin.Id) ||
                             (cm.SenderId == admin.Id && cm.ReceiverId == student.Id))
                .OrderBy(cm => cm.Timestamp)
                .ToList();

            ViewBag.AdminId = admin.Id;
            ViewBag.StudentId = student.Id;

            return View(chatMessages);
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] ChatMessageModel messageModel)
        {
            var studentNo = User.Claims.FirstOrDefault(c => c.Type == "StudentNo")?.Value;
            if (string.IsNullOrEmpty(studentNo))
            {
                return BadRequest("未登录");
            }

            var student = _context.Students.FirstOrDefault(s => s.Sno == studentNo);
            if (student == null)
            {
                return BadRequest("用户不存在");
            }

            var dormitory = _context.Dormitories.FirstOrDefault(d => d.Id == student.DId);
            if (dormitory == null)
            {
                return BadRequest("宿舍不存在");
            }

            var admin = _context.Admins.FirstOrDefault(a => a.Id == dormitory.AId);
            if (admin == null)
            {
                return BadRequest("管理员不存在");
            }

            var chatMessage = new ChatMessage
            {
                SenderId = student.Id,
                ReceiverId = admin.Id,
                Message = messageModel.Message,
                Timestamp = DateTime.Now
            };

            _context.ChatMessages.Add(chatMessage);
            _context.SaveChanges();

            return Ok();
        }
    }


}
