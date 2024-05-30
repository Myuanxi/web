using Microsoft.AspNetCore.Mvc;

namespace dms.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
