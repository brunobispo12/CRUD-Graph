using ApiGraph.src.domain;
using Microsoft.Graph.Models;

namespace ApiGraph.src.application.services
{
    public interface ISharepointService
    {
        Task<List<Dictionary<string, object>>?> ReadItems();

        Task<ListItem?> CreateItem(ToDoItem item);

        Task<FieldValueSet?> UpdateItem(String ItemId, ToDoItem item);

        Task<bool> DeleteItem(String itemId);

        Task<ListItem?> ReadItem(String itemId);
    }
}
