using Microsoft.AspNetCore.Mvc;

namespace GameShop.Controllers;

public class AdminController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}