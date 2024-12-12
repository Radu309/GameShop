using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameShop.Models.Enum;
using Microsoft.AspNetCore.Identity;

namespace GameShop.Models;

public class AppUser : IdentityUser
{
    [Required]
    public string? FullName { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Review> Reviews { get; set; }  = new List<Review>();
}