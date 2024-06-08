using Microsoft.AspNetCore.Mvc;
using dms.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
        public async Task<IActionResult> Login(string username, string password)
        {
            // 查询管理员表
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Aname == username);
            if (admin != null && admin.Password == password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                    new Claim(ClaimTypes.Name, admin.Aname),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Admin");
            }

            // 查询学生表
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Sno == username);
            if (student != null && student.Password == password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, student.Sname),
                    new Claim("StudentNo", student.Sno),
                    new Claim(ClaimTypes.Role, "Student")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Student");
            }

            // 登录失败，返回登录页面
            ViewBag.ErrorMessage = "用户名或密码错误，请重新输入。";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
