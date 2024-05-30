using Microsoft.AspNetCore.Mvc;
using dms.Models;
using Microsoft.EntityFrameworkCore;

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
            // 根据用户名和密码进行验证，假设验证成功
            bool isAdmin = true; // 假设是管理员登录
            bool isStudent = false; // 假设不是学生登录

            if (isAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (isStudent)
            {
                return RedirectToAction("Index", "Student");
            }
            else
            {
                // 其他情况，例如用户名密码错误，返回登录页面
                return RedirectToAction("Login");
            }
        }
    }
}
