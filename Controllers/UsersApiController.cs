using GameShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameShop.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/users")]
[ApiController]
public class UsersApiController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<UsersApiController> _logger;

    public UsersApiController(UserManager<AppUser> userManager, ILogger<UsersApiController> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        return Ok(users);
    }

    // GET: api/users/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning($"User with ID {id} not found.");
            return NotFound();
        }
        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] AppUser model)
    {
        if (model == null)
        {
            return BadRequest("User data is invalid.");
        }

        var user = new AppUser
        {
            UserName = model.UserName,
            FullName = model.FullName,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, "Password123!"); // Poți să modifici parola

        if (result.Succeeded)
        {
            _logger.LogInformation($"User {model.UserName} created successfully.");
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        _logger.LogError($"Failed to create user {model.UserName}: {string.Join(", ", result.Errors)}");
        return BadRequest(result.Errors);
    }

    // PUT: api/users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] AppUser model)
    {
        if (id != model.Id)
        {
            return BadRequest("User ID mismatch.");
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.FullName = model.FullName ?? user.FullName;
        user.Email = model.Email ?? user.Email;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }

    // DELETE: api/users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            _logger.LogInformation($"User {user.UserName} deleted successfully.");
            return NoContent();
        }

        return BadRequest(result.Errors);
    }
}