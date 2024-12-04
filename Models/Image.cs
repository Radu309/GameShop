using GameShop.Models.Enums;

namespace GameShop.Models;

public class Image
{
    public int Id { get; set; }
    public byte[] Data { get; set; } 
    public string FileName { get; set; }  

    // Relație
    public Game Game { get; set; }
    public int GameId { get; set; }  // ID-ul produsului asociat

}