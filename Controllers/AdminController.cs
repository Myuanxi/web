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
                List<Dormitory> dormitories;
                try
                {
                    dormitories = _context.Dormitories
                        .Where(d => d.AId == buildingNumber.Value && d.Num == dormNumber.Value)
                        .ToList();
                }
                catch (Exception ex)
                {
                    // 处理异常并记录日志
                    // Log the exception (ex)
                    return View(); // 返回一个错误视图或者显示错误信息
                }
                return View(dormitories);
            }
            return View();
        }
    }
}
