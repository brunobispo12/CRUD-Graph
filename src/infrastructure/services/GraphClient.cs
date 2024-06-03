using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ExternalConnectors;
using System.ComponentModel.Design;
using Azure.Identity;

namespace ApiGraph.src.infrastructure.services
{
    public class GraphClient
    {

        public GraphServiceClient Graph {  get; private set; }

        public GraphClient(String tenantId, String clientId, String clientSecret)
        {
            Graph = CreateGraphClient(tenantId, clientId, clientSecret);
        }

        public GraphServiceClient CreateGraphClient(String tenantId, String clientId, String clientSecret)
        {
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            return new GraphServiceClient(clientSecretCredential, scopes);
        }

    }
}
