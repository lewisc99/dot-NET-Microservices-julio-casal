using System;

namespace Play.Catalog.Contracts
{
    public record CatalogItemCreated(Guid itemId, string Name, string Description);

    //and then we need to specify the the event for when items are getting updated so i'll just copy the one for created
    public record CatalogLogItemUpdated(Guid itemId, string Name, string Description);

    public record CatalogItemDeleted(Guid itemId);
}