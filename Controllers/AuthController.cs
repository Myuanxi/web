using Microsoft.AspNetCore.Mvc;
using dms.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace dms.Controllers
{
    public class AuthController : Controller
    {
        private readonly DmsContext _context;

        public AuthController(DmsContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // 查询管理员表
            var admin = _context.Admins.FirstOrDefault(a => a.Aname == username); // 这里修改为 a.Aname
            if (admin != null && admin.Password == password)
            {
                // 管理员登录成功
                return RedirectToAction("Index", "Admin");
            }

            // 查询学生表
            var student = _context.Students.FirstOrDefault(s => s.Sno == username);
            if (student != null && student.Password == password)
            {
                // 学生登录成功
                return RedirectToAction("Index", "Student");
            }

            // 登录失败，返回登录页面
            ViewBag.ErrorMessage = "用户名或密码错误，请重新输入。";
            return View();
        }

    }
}
