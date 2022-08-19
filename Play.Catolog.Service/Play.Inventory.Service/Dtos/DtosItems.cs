using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Dtos
{
    public class DtosItems
    {



        public record GrantItemsDto(Guid userId,Guid CatalogItemId, int Quantity);

        public record InventoryItemDto(Guid CatalogItemId, string Name, string Description, int Quantity, DateTimeOffset AcquiredDate);

        public record CatalogItemDto(Guid id, string Name, string Description);

    }
}
