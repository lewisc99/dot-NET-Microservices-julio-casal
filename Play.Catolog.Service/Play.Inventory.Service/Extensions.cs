using Play.Inventory.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Play.Inventory.Service.Dtos.DtosItems;

namespace Play.Inventory.Service
{
    public static class Extensions
    {

        public static InventoryItemDto asDto(this InventoryItem item, string name, string description)
        {
            return new InventoryItemDto(item.CatalogItemId, name, description, item.Quantity, item.AcquiredDate);
        }
    }
}
