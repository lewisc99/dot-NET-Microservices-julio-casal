using Play.Catolog.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Play.Catolog.Service.Models.Dtos.Dtos;

namespace Play.Catolog.Service.Models.Dtos
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}
