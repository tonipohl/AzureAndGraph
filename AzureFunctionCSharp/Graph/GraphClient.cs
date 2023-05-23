using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Tavis.UriTemplates;

namespace GraphSample.Graph
{
    public class GraphClient
    {
        private static readonly string AuthEndpoint = "https://login.microsoftonline.com/";
        // Graph .NET quick start tpe5
        private static readonly string ClientId = "<your-appid>";
        private static readonly string ClientSecret = "<your-clientsecret>";
        private static readonly string TenantId = "<your-tenantid>";

        private const string UserCommonProperties = "id,businessPhones,displayName,givenName,jobTitle,mail,mobilePhone,officeLocation,preferredLanguage,surname,userPrincipalName";
        private const string UserAdditionalProperties = "otherMails,onPremisesExtensionAttributes,proxyAddresses,userType,externalUserState,externalUserStateChangeDateTime,assignedLicenses,assignedPlans,accountEnabled,city,companyName,country,department,mailNickname,usageLocation,streetAddress,state,postalCode,onPremisesSyncEnabled,passwordPolicies";

        private static ClientSecretCredential? _clientSecretCredential;
        // Client configured with app-only authentication
        private static GraphServiceClient? _appClient;

        public static void EnsureGraphForAppOnlyAuth()
        {
            if (_clientSecretCredential == null)
            {
                _clientSecretCredential = new ClientSecretCredential(
                TenantId, ClientId, ClientSecret);
            }
            if (_appClient == null)
            {
                _appClient = new GraphServiceClient(_clientSecretCredential,
                // Use the default scope, which will request the scopes
                // configured on the app registration
                    new[] { "https://graph.microsoft.com/.default" });
            }
        }

        public static async Task<List<User>> GetUsersAsync()
        {
            int count = 0;
            int pauseAfter = 20;

            List<User> users = new List<User>();

            // Microsoft Graph .NET SDK v5, now generally available (GA)
            // There are some breaking changes including types in new namespaces/usings and also some changes around authentication. https://www.thomy.tech/teamsfx-graph-sdk-v5/
            // Do the Authentication
            EnsureGraphForAppOnlyAuth();

            // Ensure client isn't null
            _ = _appClient ?? throw new System.NullReferenceException("Graph has not been initialized for app-only auth");

            UserCollectionResponse result = await _appClient.Users.GetAsync((requestConfiguration) =>
                {
                    requestConfiguration.QueryParameters.Select = new string[] { $"{UserCommonProperties},{UserAdditionalProperties}" };
                    // Set the PageSize
                    requestConfiguration.QueryParameters.Top = 5;
                });

            var pageIterator = PageIterator<User, UserCollectionResponse>
                                .CreatePageIterator(_appClient, result,
                                    (m) =>
                                    {
                                        users.Add(m);
                                        count++;
                                        // If we've iterated over the limit,
                                        // stop the iteration by returning false
                                        Console.WriteLine($"User: {m.DisplayName}");
                                        return count < pauseAfter;
                                    }
                                );

            // Get the first page
            await pageIterator.IterateAsync();

            // Do until finished
            while (pageIterator.State != PagingState.Complete)
            {
                Console.WriteLine($"Iteration paused... currently having {users.Count}");
                await Task.Delay(1000);
                // Reset count
                count = 0;
                await pageIterator.ResumeAsync();
            }

            return users;
        }

    }
}
