using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using Hl7.Fhir.Rest;

namespace FhirApp
{
    public class FhirService
    {
        private readonly FhirClient _fhirClient;

        public FhirService(string fhirServerUrl)
        {
            // Get the secret from KeyVault
            var keyVaultName = "health-data-keyvault";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var kvClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            // Retrieve all the required secrets
            KeyVaultSecret secret = kvClient.GetSecret("azureAdInstance");
            string AzureAdInstance = secret.Value;
            secret = kvClient.GetSecret("tenantId");
            string TenantId = secret.Value;
            secret = kvClient.GetSecret("clientId");
            string ClientId = secret.Value;
            secret = kvClient.GetSecret("clientSecret");
            string ClientSecret = secret.Value;
            secret = kvClient.GetSecret("fhirScope");
            string FhirScope = secret.Value;

            // Create a ConfidentialClientApplication instance
            var app = ConfidentialClientApplicationBuilder.Create(ClientId)
                .WithClientSecret(ClientSecret)
                .WithAuthority(new Uri($"{AzureAdInstance}{TenantId}"))
                .Build();

            // Acquire a token for the specified scopes
            var bearerToken = app.AcquireTokenForClient(new string[] { FhirScope }).ExecuteAsync().Result;

            // Instanciate the authorization handler
            var authHandler = new AuthorizationMessageHandler
            {
                Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.AccessToken)
            };

            // Create client
            var settings = new FhirClientSettings
            {
                PreferredFormat = ResourceFormat.Json,
                PreferredReturn = Prefer.ReturnMinimal,
                Timeout = 10000,
                VerifyFhirVersion = true,
            };
            _fhirClient = new FhirClient(fhirServerUrl, settings, authHandler);
        }

        public T Execute<T>(Func<FhirClient, T> fhirFunction)
        {
            try
            {
                return fhirFunction(_fhirClient);
            }
            catch (FhirOperationException ex)
            {
                throw new Exception($"FHIR operation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to communicate with FHIR server: {ex.Message}");
            }
        }
    }
}