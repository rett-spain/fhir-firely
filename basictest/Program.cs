using Hl7.Fhir.Model;

namespace FhirApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a new FHIR service
            Console.WriteLine("Connecting with FHIR Service...");
            var fhirService = new FhirService("https://aesrworkspace1-rettregistry.fhir.azurehealthcareapis.com");

            // Create a new patient service
            var patientService = new PatientService(fhirService);

            while (true)
            {
                // Display menu options
                Console.WriteLine("MAIN MENU:");
                Console.WriteLine("1. Create a new patient");
                Console.WriteLine("2. List all patients");
                Console.WriteLine("3. Search a patient by name");
                Console.WriteLine("4. Exit");

                // Get user input
                Console.Write("Enter option number: ");
                var input = Console.ReadLine();

                // Execute the option
                switch (input)
                {
                    case "1":
                        Console.Write("Enter first name: ");
                        var firstName = Console.ReadLine();
                        Console.Write("Enter middle name: ");
                        var middleName = Console.ReadLine();
                        Console.Write("Enter family name: ");
                        var familyName = Console.ReadLine();
                        Console.Write("Enter birth date (YYYY-MM-DD): ");
                        var birthDate = Console.ReadLine();
                        var newPatient = patientService.CreatePatient(firstName, middleName, familyName, birthDate);
                        Console.WriteLine("Patient created:");
                        break;
                    case "2":
                        var patients = patientService.GetAllPatients();
                        foreach (var patient in patients)
                        {
                            Console.WriteLine($"Patient id {patient.Id}: {patient.Name[0].Given.FirstOrDefault()} {patient.Name[0].Family}");
                        }
                        break;
                    case "3":
                        Console.Write("Enter name to search: ");
                        var searchName = Console.ReadLine();
                        var searchResult = patientService.SearchName (searchName);
                        foreach (var result in searchResult.Entry)
                        {
                            var patient = result.Resource as Patient;
                            Console.WriteLine($"Patient id {patient.Id}: {patient.Name[0].Given.FirstOrDefault()} {patient.Name[0].Family}");
                        }
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }

                // Wait for user input to continue
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }

            /*
                        var patient = new Patient()
                        {
                            Name = new List<HumanName>()
                            {
                                new HumanName()
                                {
                                    Given = new List<string> { "John", "James" },
                                    Family = "New Doe 2"
                                }
                            },
                            Gender = AdministrativeGender.Male,
                            BirthDate = "1990-01-01",
                            Identifier = new List<Identifier>()
                            {
                                new Identifier()
                                {
                                    System = "http://acme.org/mrns",
                                    Value = "12345"
                                }
                            }
                        };

                        // Write the patient to the FHIR server
                        Console.WriteLine($"Sending patient {patient.Name[0].Given.FirstOrDefault()} {patient.Name[0].Family} to the FHIR server");

                        var settings = new FhirClientSettings
                        {
                            PreferredFormat = ResourceFormat.Json,
                            PreferredReturn = Prefer.ReturnMinimal,
                            Timeout = 10000,
                            VerifyFhirVersion = true,
                        };

                        // Get the secret from KeyVault
                        var keyVaultName = "health-data-keyvault";
                        var kvUri = $"https://{keyVaultName}.vault.azure.net";
                        var kvClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

                        // Retrieve all the required secrets
                        KeyVaultSecret secret = await kvClient.GetSecretAsync("azureAdInstance");
                        string AzureAdInstance = secret.Value;
                        secret = await kvClient.GetSecretAsync("tenantId");
                        string TenantId = secret.Value;
                        secret = await kvClient.GetSecretAsync("clientId");
                        string ClientId = secret.Value;
                        secret = await kvClient.GetSecretAsync("clientSecret");
                        string ClientSecret = secret.Value;
                        secret = await kvClient.GetSecretAsync("fhirScope");
                        string FhirScope = secret.Value;

                        // Create a ConfidentialClientApplication instance
                        var app = ConfidentialClientApplicationBuilder.Create(ClientId)
                            .WithClientSecret(ClientSecret)
                            .WithAuthority(new Uri($"{AzureAdInstance}{TenantId}"))
                            .Build();

                        // Acquire a token for the specified scopes
                        var bearerToken = await app.AcquireTokenForClient(new string[] { FhirScope }).ExecuteAsync();

                        // Instanciate the authorization handler
                        var authHandler = new AuthorizationMessageHandler();
                        authHandler.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken.AccessToken);

                        // Create client
                        var client = new FhirClient("https://aesrworkspace1-rettregistry.fhir.azurehealthcareapis.com", settings, authHandler);

                        // Create a patient
                        client.Create(patient);
                        Console.WriteLine($"Result: {client.LastResult.Status}");

                        var searchResult = client.Search("Patient", new string[] { "name=John" });
                        foreach (var result in searchResult.Entry)
                        {
                            var pat = result.Resource as Patient;
                            Console.WriteLine($"Received patient with {pat.Name[0].Given.FirstOrDefault()} {pat.Name[0].Family}");
                        }
            */
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