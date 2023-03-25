using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Hl7.Fhir.Rest;

namespace FhirApp
{
    public class FhirService
    {
        private readonly FhirClient _fhirClient;

        public FhirService()
        {
            // Get the FHIR server URL and KeyVault name from the configuration file
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var FhirUrl = configuration["FhirUrl"];
            var keyVaultName = configuration["KeyVaultName"];

            // Get the secret from KeyVault
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
            _fhirClient = new FhirClient(FhirUrl, settings, authHandler);
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


public class AuthorizationMessageHandler : HttpClientHandler
{
    public System.Net.Http.Headers.AuthenticationHeaderValue? Authorization { get; set; }
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (Authorization != null)
            request.Headers.Authorization = Authorization;
        return await base.SendAsync(request, cancellationToken);
    }
}