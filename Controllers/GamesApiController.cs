using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameShop.Data;
using GameShop.Models;

namespace GameShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesApiController : ControllerBase
{
    private readonly GameShopContext _context;

    public GamesApiController(GameShopContext context)
    {
        _context = context;
    }

    // GET: api/GamesApi
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game>>> GetGames()
    {
        var games = await _context.Games
            .Include(g => g.Images).ToListAsync();
        
        // Proiectăm răspunsul pentru a include imaginile în format Base64
        var gamesWithImages = games.Select(game => new
        {
            game.Id,
            game.Name,
            game.Description,
            game.Price,
            game.Stock,
            Images = game.Images.Select(img => new
            {
                img.Id,
                Base64Data = img.Data != null ? ConvertImageToBase64(img.Data, "image/jpeg") : null,
                img.FileName
            })
        });

        return Ok(gamesWithImages);
    }

    // GET: api/GamesApi/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGame(int id)
    {
        var game = await _context.Games.FindAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        return game;
    }

    // PUT: api/GamesApi/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(int id, Game game)
    {
        if (id != game.Id)
        {
            return BadRequest();
        }

        _context.Entry(game).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GameExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/GamesApi
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Game>> PostGame(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetGame", new { id = game.Id }, game);
    }

    // DELETE: api/GamesApi/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GameExists(int id)
    {
        return _context.Games.Any(e => e.Id == id);
    }
    private string ConvertImageToBase64(byte[] imageData, string contentType)
    {
        return $"data:{contentType};base64,{Convert.ToBase64String(imageData)}";
    }

    
}

