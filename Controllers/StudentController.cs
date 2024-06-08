using Microsoft.AspNetCore.Mvc;
using dms.Models;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System;

namespace dms.Controllers
{
    public class StudentController : Controller
    {
        private readonly DmsContext _context;
        private const int PageSize = 10; // 每页显示的消息数

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
                .Take(PageSize)
                .ToList();

            ViewBag.AdminId = admin.Id;
            ViewBag.StudentId = student.Id;

            return View(chatMessages);
        }

        public IActionResult GetChatMessages(int pageIndex)
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

            var chatMessages = _context.ChatMessages
                .Where(cm => (cm.SenderId == student.Id && cm.ReceiverId == admin.Id) ||
                             (cm.SenderId == admin.Id && cm.ReceiverId == student.Id))
                .OrderBy(cm => cm.Timestamp)
                .Skip(pageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            return Json(chatMessages);
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

            var student = _context.Students.FirstOrDefault(s => s.Sno == studentNo);
            if (student == null)
            {
                return RedirectToAction("Login", "Auth");
            }

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
