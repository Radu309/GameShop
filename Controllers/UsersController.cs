using GameShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameShop.Controllers;

public class UsersController : Controller
{
    private readonly GameShopContext _context;

    public UsersController(GameShopContext context)
    {
        _context = context;
    }

    // GET: /users
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.ToListAsync();  
        return View(users);  
    }
}