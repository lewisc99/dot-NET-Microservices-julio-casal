using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static Play.Inventory.Service.Dtos.DtosItems;

namespace Play.Inventory.Service.Clients
{
    public class CatalogClient
    {

        private readonly HttpClient httpClient;


        public CatalogClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        //retrieve the items from catalog

        public async Task<IReadOnlyCollection<CatalogItemDto>> getCatalogItemsAsync()
        {
            var items = await httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items");
            return items;
        }

    }
}
