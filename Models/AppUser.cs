using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GameShop.Models;

public class AppUser : IdentityUser
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Phone { get; set; }
    public UserRole Role { get; set; } = UserRole.CUSTOMER;

    // Relationships
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Review> Reviews { get; set; }  = new List<Review>();
}