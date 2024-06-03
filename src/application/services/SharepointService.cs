using ApiGraph.src.domain;
using ApiGraph.src.infrastructure.services;
using Microsoft.Graph.Models;

namespace ApiGraph.src.application.services
{
    public class SharepointService : ISharepointService
    {
        private readonly GraphClient _graphClient;
        private readonly string _siteId;
        private readonly string _listId;

        public SharepointService(GraphClient graphClient, IConfiguration configuration)
        {
            _graphClient = graphClient;
            _siteId = configuration["MicrosoftGraph:SiteId"] ?? "";
            _listId = configuration["MicrosoftGraph:ListId"] ?? "";
        }

        public async Task<List<Dictionary<string, object>>?> ReadItems()
        {
            var result = (await _graphClient.Graph.Sites[_siteId].Lists[_listId].Items.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Expand = new string[] { "fields($select=*)" };
            }))?.Value;

            if (result == null) { return null; }

            List<Dictionary<string, object>> fieldsList = new List<Dictionary<string, object>>();

            foreach (var item in result)
            {
                if (item.Fields?.AdditionalData != null)
                {
                    fieldsList.Add(new Dictionary<string, object>(item.Fields.AdditionalData));
                }
            }

            return fieldsList;
        }

        public async Task<ListItem?> CreateItem(ToDoItem item)
        {
            var (Titulo, Descricao, Realizado) = item;

            var requestBody = new ListItem
            {
                Fields = new FieldValueSet
                {
                    AdditionalData = new Dictionary<string, object>
                    {
                        { "Title", Titulo ?? "" },
                        { "Descricao", Descricao ?? "" },
                        { "Realizado", Realizado ?? false }
                    }
                }
            };

            var response = await _graphClient.Graph.Sites[_siteId].Lists[_listId].Items.PostAsync(requestBody);

            if (response == null) { return null; }

            return response;
        }

        public async Task<FieldValueSet?> UpdateItem(string itemId, ToDoItem item)
        {
            var requestBody = new FieldValueSet
            {
                AdditionalData = new Dictionary<string, object>()
            };

            // Adiciona apenas os campos que não são nulos
            if (!string.IsNullOrEmpty(item.Titulo))
            {
                requestBody.AdditionalData.Add("Title", item.Titulo);
            }

            if (!string.IsNullOrEmpty(item.Descricao))
            {
                requestBody.AdditionalData.Add("Descricao", item.Descricao);
            }

            if (item.Realizado.HasValue)
            {
                requestBody.AdditionalData.Add("Realizado", item.Realizado.Value);
            }

            var response = await _graphClient.Graph.Sites[_siteId].Lists[_listId].Items[itemId].Fields
                .PatchAsync(requestBody);

            if (response == null) { return null; }

            return response;
        }

        public async Task<bool> DeleteItem(string itemId)
        {
            await _graphClient.Graph.Sites[_siteId].Lists[_listId].Items[itemId]
                .DeleteAsync();

            return true;
        }

        public async Task<ListItem?> ReadItem(string itemId)
        {
            var response = await _graphClient.Graph.Sites[_siteId].Lists[_listId].Items[itemId].GetAsync();

            if (response == null) { return null; };

            return response;
        }
    }
}
