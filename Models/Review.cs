namespace GameShop.Models;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }  // Punctajul (1-5)
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }

    // Relație - O recenzie este asociată unui produs
    public int GameId { get; set; }
    public Game? Game { get; set; }

    // Relație - O recenzie este făcută de un client
    public int ClientId { get; set; }
    public Client? Client { get; set; }
}