using GameShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameShop.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public HomeController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        
        if (user == null)
            return RedirectToAction("Login", "Account");
        
        if (await _userManager.IsInRoleAsync(user, "Admin"))
            return RedirectToAction("Index", "Games");
        
        if (await _userManager.IsInRoleAsync(user, "Customer"))
            return RedirectToAction("Index", "AdminGames");
        return RedirectToAction("Index", "Home");
    }
}