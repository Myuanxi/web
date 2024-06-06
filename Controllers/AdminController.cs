using Microsoft.AspNetCore.Mvc;
using dms.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dms.Controllers
{
    public class AdminController : Controller
    {
        private readonly DmsContext _context;
        private const int PageSize = 10; // 每页显示的项目数

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
                List<Room> rooms;
                try
                {
                    rooms = _context.Rooms
                        .Where(r => r.DId == buildingNumber.Value && r.Num == dormNumber.Value)
                        .Skip(pageIndex * PageSize)
                        .Take(PageSize)
                        .ToList();

                    var totalRooms = _context.Rooms.Count(r => r.DId == buildingNumber.Value && r.Num == dormNumber.Value);
                    ViewBag.HasPreviousPage = pageIndex > 0;
                    ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalRooms;
                    ViewBag.PageIndex = pageIndex;
                }
                catch (Exception ex)
                {
                    // 处理异常并记录日志
                    // Log the exception (ex)
                    return View(); // 返回一个错误视图或者显示错误信息
                }
                return View(rooms);
            }
            return View();
        }

        public IActionResult StudentManagement(int pageIndex = 0)
        {
            var totalStudents = _context.Students.Count();
            var students = _context.Students
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
            var totalInOuts = _context.InOuts.Count();
            var inOuts = _context.InOuts
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalInOuts;
            ViewBag.PageIndex = pageIndex;

            return View(inOuts);
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

            var totalInOuts = query.Count();
            var inOutRecords = query
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalInOuts;
            ViewBag.PageIndex = pageIndex;

            return View(inOutRecords);
        }

        public IActionResult VisitorRegistration(int pageIndex = 0)
        {
            var totalVisits = _context.Visits.Count();
            var visits = _context.Visits
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
            var visitRecords = query
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.HasPreviousPage = pageIndex > 0;
            ViewBag.HasNextPage = (pageIndex + 1) * PageSize < totalVisits;
            ViewBag.PageIndex = pageIndex;

            return View(visitRecords);
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
    }
}
