using Microsoft.AspNetCore.Mvc;

namespace GameShop.Controllers;

public class UsersController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}