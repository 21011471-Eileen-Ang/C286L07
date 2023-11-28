using Microsoft.AspNetCore.Mvc;

namespace Lesson07.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

}

