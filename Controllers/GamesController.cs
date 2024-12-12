using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameShop.Data;
using GameShop.Models;
using GameShop.Services;
using Microsoft.AspNetCore.Authorization;

namespace GameShop.Controllers;

[Authorize(Roles = "Customer, Admin")]
public class GamesController : Controller
{
    private readonly GameShopContext _context;
    private readonly GameService _gameService;

    public GamesController(GameShopContext context, GameService gameService)
    {
        _context = context;
        _gameService = gameService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var games = _gameService.GetAllGames();
        ViewBag.IsAdmin = User.IsInRole("Admin"); 
        return View(games);
    }

    public async Task<IActionResult> Details(int? id) =>
        await GetGameById(id) is Game game ? View(game) : NotFound();

    [Authorize(Roles = "Admin")] // Doar Admin poate crea jocuri
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Game game, IFormFile[] imageFiles)
    {
        if (!ModelState.IsValid) return View(game);
        if (imageFiles != null && imageFiles.Length > 0)
        {
            if (!await ProcessImages(game, imageFiles)) return View(game);
        }
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult GetImage(int id) =>
        _context.Images.Find(id) is Image image ? File(image.Data, "image/jpeg") : NotFound();

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id) =>
        await GetGameById(id) is Game game ? View(game) : NotFound();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Game updatedGame, IFormFile[] imageFiles)
    {
        if (id != updatedGame.Id || !ModelState.IsValid) return View(updatedGame);

        var game = await _context.Games.Include(g => g.Images).FirstOrDefaultAsync(g => g.Id == id);
        if (game == null) return NotFound();
        UpdateGameDetails(game, updatedGame);
        if (!await ProcessImages(game, imageFiles)) return View(game);
        _context.Update(game);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteImage(int imageId, int gameId)
    {
        if (_context.Images.Find(imageId) is not Image image) return NotFound();
        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Edit), new { id = gameId });
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id) =>
        await GetGameById(id) is Game game ? View(game) : NotFound();

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Games.Find(id) is Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return NotFound();
    }

    private bool GameExists(int id) => _context.Games.Any(e => e.Id == id);

    private async Task<Game?> GetGameById(int? id) =>
        id == null ? null : await _context.Games
            .Include(g => g.Images)
            .Include(g => g.Reviews)
            .ThenInclude(r => r.AppUser)
            .FirstOrDefaultAsync(m => m.Id == id);

    private void UpdateGameDetails(Game existingGame, Game updatedGame)
    {
        existingGame.Name = updatedGame.Name;
        existingGame.Description = updatedGame.Description;
        existingGame.Price = updatedGame.Price;
        existingGame.Stock = updatedGame.Stock;
    }

    private async Task<bool> ProcessImages(Game game, IFormFile[] imageFiles)
    {
        if (imageFiles == null || imageFiles.Length == 0)
        {
            ModelState.AddModelError("imageFiles", "At least one image is required.");
            return false;
        }
        foreach (var file in imageFiles)
        {
            if (file.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("imageFiles", "Only JPEG images are allowed.");
                return false;
            }
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            game.Images.Add(new Image { FileName = file.FileName, Data = memoryStream.ToArray() });
        }
        return true;
    }
}
