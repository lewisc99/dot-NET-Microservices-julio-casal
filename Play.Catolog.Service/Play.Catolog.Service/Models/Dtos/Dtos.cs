using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catolog.Service.Models.Dtos
{
    public class Dtos
    {
        public record ItemDto(Guid id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);


      
        public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price,  DateTimeOffset CreatedDate);

        public record UpdateItemDto([Required]  string Name, string Description, [Range(0, 1000)] decimal Price);

    }
}
