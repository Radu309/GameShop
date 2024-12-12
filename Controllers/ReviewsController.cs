using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameShop.Data;
using GameShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace GameShop.Controllers;

public class ReviewsController : Controller
{
    private readonly GameShopContext _context;

    public ReviewsController(GameShopContext context)
    {
        _context = context;
    }

    // GET: Reviews
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var gameShopContext = _context.Reviews.Include(r => r.AppUser).Include(r => r.Game);
        ViewBag.IsAdmin = User.IsInRole("Admin");
        return View(await gameShopContext.ToListAsync());
    }

    // GET: Reviews/Details/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Reviews
            .Include(r => r.AppUser)
            .Include(r => r.Game)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }
    [Authorize(Roles = "Customer, Admin")]
    // GET: Reviews/Create
    public IActionResult Create(int gameId, string appUserId)
    {
        var review = new Review
        {
            GameId = gameId,
            AppUserId = appUserId
        };
        ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id");
        ViewData["GameId"] = new SelectList(_context.Games, "Id", "Description");
        // return RedirectToAction("Index", "Games");
        return View(review);
    }

    // POST: Reviews/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Customer, Admin")]
    public async Task<IActionResult> Create([Bind("Id,Rating,Comment,ReviewDate,GameId,AppUserId")]
        Review review)
    {
        if (ModelState.IsValid)
        {
            Console.WriteLine("HEREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            review.ReviewDate = DateTime.UtcNow;
            _context.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Games");
        }
        ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", review.AppUserId);
        ViewData["GameId"] = new SelectList(_context.Games, "Id", "Description", review.GameId);
        // return View(review);
        return RedirectToAction("Index", "Games");
    }

    // GET: Reviews/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }
        ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", review.AppUserId);
        ViewData["GameId"] = new SelectList(_context.Games, "Id", "Description", review.GameId);
        return View(review);
    }

    // POST: Reviews/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Rating,Comment,ReviewDate,GameId,AppUserId")] Review review)
    {
        if (id != review.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(review);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(review.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", review.AppUserId);
        ViewData["GameId"] = new SelectList(_context.Games, "Id", "Description", review.GameId);
        return View(review);
    }

    // GET: Reviews/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Reviews
            .Include(r => r.AppUser)
            .Include(r => r.Game)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }

    // POST: Reviews/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review != null)
        {
            _context.Reviews.Remove(review);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReviewExists(int id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }
}
