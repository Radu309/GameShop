using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameShop.Data;
using GameShop.Models;
using GameShop.Models.Enums;
using SixLabors.ImageSharp.Formats.Jpeg;
using _Image = SixLabors.ImageSharp.Image;

namespace GameShop.Controllers;
public class GamesController : Controller
{
    private readonly GameShopContext _context;

    public GamesController(GameShopContext context)
    {
        _context = context;
    }

    // GET: Games
    public async Task<IActionResult> Index()
    {
        var games = await _context.Games.Include(g => g.Images).ToListAsync();
        return View(await _context.Games.ToListAsync());
    }

    // GET: Games/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var game = await _context.Games
            .FirstOrDefaultAsync(m => m.Id == id);
        if (game == null)
        {
            return NotFound();
        }

        return View(game);
    }
    
    // GET: Games/Create
    public IActionResult Create()
    {
        return View();
    }
    
    // POST: Games/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Game game, IFormFile[] imageFiles)
    {
        if (ModelState.IsValid)
        {
            // Inițializați lista de imagini
            game.Images = new List<Image>();

            if (imageFiles != null && imageFiles.Length > 0)
            {
                foreach (var imageFile in imageFiles)
                {
                    if (imageFile.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("imageFiles", "Only JPEG images are allowed.");
                        return View(game); // Dacă o imagine nu este validă, returnează formularul cu eroare
                    }

                    using var memoryStream = new MemoryStream();
                    await imageFile.CopyToAsync(memoryStream);

                    var image = new Image
                    {
                        FileName = imageFile.FileName,
                        Data = memoryStream.ToArray()
                    };

                    game.Images.Add(image); // Adaugă fiecare imagine în colecția asociată jocului
                }
            }
            else
            {
                ModelState.AddModelError("imageFiles", "At least one image is required.");
                return View(game); // Dacă nu s-au selectat imagini, returnează formularul cu eroare
            }

            _context.Games.Add(game); // Adaugă jocul în baza de date
            await _context.SaveChangesAsync(); // Salvează schimbările

            return RedirectToAction("Index"); // Redirecționează utilizatorul la lista de jocuri
        }

        return View(game); // Dacă validarea nu este validă, rămâi pe formular
    }
    // GET: Games/GetImage/5
    public IActionResult GetImage(int id)
    {
        var image = _context.Images.FirstOrDefault(img => img.Id == id);
        if (image == null)
        {
            return NotFound();
        }
        return File(image.Data, "image/jpeg");
    }

    
    // GET: Games/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        return View(game);
    }

    // POST: Games/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Stock")] Game game)
    {
        if (id != game.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(game);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(game.Id))
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
        return View(game);
    }

    // GET: Games/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var game = await _context.Games
            .FirstOrDefaultAsync(m => m.Id == id);
        if (game == null)
        {
            return NotFound();
        }

        return View(game);
    }

    // POST: Games/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            _context.Games.Remove(game);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GameExists(int id)
    {
        return _context.Games.Any(e => e.Id == id);
    }
}
