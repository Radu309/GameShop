using GameShop.Data;
using GameShop.Models.DTO;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;
using SixLabors.ImageSharp;

namespace GameShop.Services;

public class GameService
{
    private readonly GameShopContext _context;

    public GameService(GameShopContext context)
    {
        _context = context;
    }
    public IEnumerable<GameDTO> GetAllGames()
    {
        // Preluăm jocurile din baza de date
        var games = _context.Games
            .Select(game => new
            {
                game.Id,
                game.Name,
                game.Price,
                game.Stock,
                Images = game.Images.Select(img => new
                {
                    img.FileName,
                    img.Data
                }).ToList()
            })
            .ToList();

        // Convertim imaginile în JPEG Base64
        return games.Select(game => new GameDTO
        {
            Id = game.Id,
            Name = game.Name,
            Price = game.Price,
            Stock = game.Stock,
            Images = game.Images.Select(img => new ImageDTO
            {
                FileName = img.FileName,
                Base64Data = ConvertImageToJpegBase64(img.Data)
            }).ToList()
        }).ToList();
    }
    private string ConvertImageToJpegBase64(byte[]? imageData)
    {
        if (imageData == null)
        {
            return string.Empty; // Sau un URL către o imagine placeholder
        }
    
        using (MemoryStream inputMemoryStream = new MemoryStream(imageData))
        using (MemoryStream outputMemoryStream = new MemoryStream())
        {
            using (var image = Image.Load(inputMemoryStream))
            {
                image.Save(outputMemoryStream, new JpegEncoder());
            }
    
            return $"data:image/jpeg;base64,{Convert.ToBase64String(outputMemoryStream.ToArray())}";
        }
    }

}