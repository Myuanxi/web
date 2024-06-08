using Microsoft.AspNetCore.Mvc;
using dms.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Security.Claims;

namespace dms.Controllers
{
    public class AdminController : Controller
    {
        private readonly DmsContext _context;
        private const int PageSize = 10; // 每页显示的记录数

        public AdminController(DmsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DormitoryManagement(int pageIndex = 0)
        {
            var totalRooms = _context.Rooms.Count();
            var rooms = _context.Rooms
                .OrderBy(r => r.Id)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalRooms;
            ViewBag.PageIndex = pageIndex;

            return View(rooms);
        }

        [HttpPost]
        public IActionResult DormitoryManagement(int? buildingNumber, int? dormNumber, int pageIndex = 0)
        {
            if (buildingNumber.HasValue && dormNumber.HasValue)
            {
                var query = _context.Rooms.AsQueryable();

                if (buildingNumber.HasValue)
                    query = query.Where(r => r.DId == buildingNumber.Value);

                if (dormNumber.HasValue)
                    query = query.Where(r => r.Num == dormNumber.Value);

                var totalRooms = query.Count();
                var rooms = query
                    .OrderBy(r => r.Id)
                    .Skip(pageIndex * PageSize)
                    .Take(PageSize)
                    .ToList();

                ViewBag.HasPreviousPage = pageIndex > 0;
                ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalRooms;
                ViewBag.PageIndex = pageIndex;

                return View(rooms);
            }
            return View();
        }

        public IActionResult StudentManagement(int pageIndex = 0)
        {
            var totalStudents = _context.Students.Count();
            var students = _context.Students
                .OrderBy(s => s.Id)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalStudents;
            ViewBag.PageIndex = pageIndex;

            return View(students);
        }

        [HttpPost]
        public IActionResult StudentManagement(string sno, string sname, string gender, int? age, string tel, int? u_id, int? m_id, int? @class, int? d_id, int? r_id, int pageIndex = 0)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(sno))
                query = query.Where(s => s.Sno == sno);

            if (!string.IsNullOrEmpty(sname))
                query = query.Where(s => s.Sname == sname);

            if (!string.IsNullOrEmpty(gender))
                query = query.Where(s => s.Gender == gender);

            if (age.HasValue)
                query = query.Where(s => s.Age == age.Value);

            if (!string.IsNullOrEmpty(tel))
                query = query.Where(s => s.Tel == tel);

            if (u_id.HasValue)
                query = query.Where(s => s.UId == u_id.Value);

            if (m_id.HasValue)
                query = query.Where(s => s.MId == m_id.Value);

            if (@class.HasValue)
                query = query.Where(s => s.Class == @class.Value);

            if (d_id.HasValue)
                query = query.Where(s => s.DId == d_id.Value);

            if (r_id.HasValue)
                query = query.Where(s => s.RId == r_id.Value);

            var totalStudents = query.Count();
            var students = query
                .OrderBy(s => s.Id)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalStudents;
            ViewBag.PageIndex = pageIndex;

            return View(students);
        }

        public IActionResult EntryRegistration(int pageIndex = 0)
        {
            var totalEntries = _context.InOuts.Count();
            var entries = _context.InOuts
                .OrderBy(io => io.Id)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalEntries;
            ViewBag.PageIndex = pageIndex;

            return View(entries);
        }

        [HttpPost]
        public IActionResult EntryRegistration(int? d_id, int? s_id, DateTime? out_date, DateTime? in_date, int pageIndex = 0)
        {
            var query = _context.InOuts.AsQueryable();

            if (d_id.HasValue)
                query = query.Where(io => io.DId == d_id.Value);

            if (s_id.HasValue)
                query = query.Where(io => io.SId == s_id.Value);

            if (out_date.HasValue)
                query = query.Where(io => io.OutDate >= out_date.Value);

            if (in_date.HasValue)
                query = query.Where(io => io.InDate <= in_date.Value);

            var totalEntries = query.Count();
            var entries = query
                .OrderBy(io => io.Id)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalEntries;
            ViewBag.PageIndex = pageIndex;

            return View(entries);
        }

        public IActionResult VisitorRegistration(int pageIndex = 0)
        {
            var totalVisits = _context.Visits.Count();
            var visits = _context.Visits
                .OrderBy(v => v.Id)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalVisits;
            ViewBag.PageIndex = pageIndex;

            return View(visits);
        }

        [HttpPost]
        public IActionResult VisitorRegistration(int? d_id, int? s_id, string name, DateTime? in_date, DateTime? out_date, int pageIndex = 0)
        {
            var query = _context.Visits.AsQueryable();

            if (d_id.HasValue)
                query = query.Where(v => v.DId == d_id.Value);

            if (s_id.HasValue)
                query = query.Where(v => v.SId == s_id.Value);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(v => v.Name.Contains(name));

            if (in_date.HasValue)
                query = query.Where(v => v.InDate >= in_date.Value);

            if (out_date.HasValue)
                query = query.Where(v => v.OutDate <= out_date.Value);

            var totalVisits = query.Count();
            var visits = query
                .OrderBy(v => v.Id)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalVisits;
            ViewBag.PageIndex = pageIndex;

            return View(visits);
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

        [HttpPost]
        public IActionResult Notifications(string title, string content, int pageIndex = 0)
        {
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
            {
                var notification = new Notification
                {
                    Title = title,
                    Content = content,
                    CreatedAt = DateTime.Now
                };

                _context.Notifications.Add(notification);
                _context.SaveChanges();
            }

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
            var adminId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(adminId))
                {
                    return RedirectToAction("Login", "Auth");
                }

            var admin = _context.Admins.FirstOrDefault(a => a.Id == int.Parse(adminId));
            if (admin == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var chatMessages = _context.ChatMessages
                .Where(cm => cm.ReceiverId == admin.Id || cm.SenderId == admin.Id)
                .OrderBy(cm => cm.Timestamp)
                .ToList();

            ViewBag.AdminId = admin.Id;

            return View(chatMessages);
        }

        [HttpPost]
        public IActionResult SubmitChatMessage(int receiverId, string message)
        {
            var adminId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(adminId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var chatMessage = new ChatMessage
            {
                SenderId = int.Parse(adminId),
                ReceiverId = receiverId,
                Message = message,
                Timestamp = DateTime.Now
            };

            _context.ChatMessages.Add(chatMessage);
            _context.SaveChanges();

            return RedirectToAction("Chat");
        }
    }
}

