﻿using Microsoft.AspNetCore.Mvc;

namespace dms.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
