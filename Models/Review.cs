using System.Text.Json.Serialization;

namespace GameShop.Models;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }  // Punctajul (1-5)
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }

    public int GameId { get; set; }
    [JsonIgnore] 
    public Game? Game { get; set; }
    public int AppUserId { get; set; }
    [JsonIgnore] 
    public AppUser? AppUser { get; set; }
}