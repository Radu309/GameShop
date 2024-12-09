using System.Text.Json.Serialization;

namespace GameShop.Models;

public class Image
{
    public int Id { get; set; }
    public byte[]? Data { get; set; } 
    public string? FileName { get; set; }  

    // Relație
    [JsonIgnore] // Ignoră referința inversă
    public Game? Game { get; set; }
    public int GameId { get; set; }  // ID-ul produsului asociat

}