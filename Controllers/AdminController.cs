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

        public AdminController(DmsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DormitoryManagement()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DormitoryManagement(int? buildingNumber, int? dormNumber)
        {
            if (buildingNumber.HasValue && dormNumber.HasValue)
            {
                List<Room> rooms;
                try
                {
                    rooms = _context.Rooms
                        .Where(r => r.DId == buildingNumber.Value && r.Num == dormNumber.Value)
                        .ToList();
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

        public IActionResult StudentManagement()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StudentManagement(string sno, string sname, string gender, int? age, string tel, int? u_id, int? m_id, int? @class, int? d_id, int? r_id)
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

            var students = query.ToList();
            return View(students);
        }

        public IActionResult EntryRegistration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EntryRegistration(int? d_id, int? s_id, DateTime? out_date, DateTime? in_date)
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

            var inOutRecords = query.ToList();
            return View(inOutRecords);
        }
        public IActionResult VisitorRegistration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VisitorRegistration(int? d_id, int? s_id, string name, DateTime? in_date, DateTime? out_date)
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

            var visitRecords = query.ToList();
            return View(visitRecords);
        }
    }
}
