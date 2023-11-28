using Microsoft.AspNetCore.Mvc;

namespace Lesson07.Controllers;

public class RazorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult IfElseOddOrEven(int num)
    {
        return View(num);
    }

    public IActionResult IfElseCheckPrice(double price)
    {
        return View(price);
    }

    public IActionResult ExCanYouDrive(int age)
    {
        return View(age);
    }

    public IActionResult ExHouses(int numHouses)
    {
        return View(numHouses);
    }

    public IActionResult LoopForSnackStock()
    {
        return View();
    }

    public IActionResult LoopWhileSnackStock()
    {
        return View();
    }

    public IActionResult ExForEachSnackStock()
    {
        return View();
    }

    public IActionResult ExShowLines(int lastLine)
    {
        return View(lastLine);
    }

    public IActionResult LoopIfOddNumbersUnder(int upperLimit)
    {
        return View(upperLimit);
    }

    
}
