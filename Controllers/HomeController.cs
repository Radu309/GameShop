using Microsoft.AspNetCore.Mvc;

namespace GameShop.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}