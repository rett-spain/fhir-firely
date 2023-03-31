# FHIR Console Application to test Firely .NET SDK

This is a simple .NET console application that allows users to interact with a FHIR server using the [Firely .NET SDK](https://docs.fire.ly).

We've only tested the application with the [Azure API for FHIR](https://azure.microsoft.com/en-us/services/azure-api-for-fhir/), but it should work with any FHIR server that supports OAuth2 authentication.

## Requirements

To use this application, you will need:

* .NET Core 3.1 or later
* A FHIR server to connect to. This application has been tested with the [Azure API for FHIR](https://azure.microsoft.com/en-us/services/azure-api-for-fhir/), but it should work with any FHIR server that supports OAuth2 authentication.
* A Keyvault to store the FHIR server's credentials
  * azureAdInstance (e.g. https://login.microsoftonline.com/)
  * clientSecret (client secret of the application registered in Azure AD)
  * fhirScope (scope of the FHIR server, e.g. https://&lt;your-fhir-server&gt;.azurehealthcareapis.com/.default)

## Configuration

You also need a `secrets.json` file with the followin parameters. This file is not included in the repository for security reasons. You can create it by copying the `secrets.json.example` file and filling in the following information:
* tenantId (tenant ID of the Azure AD instance)
* clientId (client ID of the application registered in Azure AD, aka Service Principal)

Update the `appsettings.json` file with the following information:
* FhirUrl: The URL of the FHIR server
* KeyVaultName: The name of the Keyvault that stores the FHIR server's credentials

## Usage

When you run the application, you will be presented with a main menu that prompts you to select an option:

```
Please select an option:
1. Create a new patient
2. List all patients
3. Search patients
4. Exit
```

To create a new patient, select option 1 and follow the prompts to enter the patient's information.

To list all patients, select option 2. The application will retrieve a list of all patients from the FHIR server and display their names, birth dates, and phone numbers.

To search for patients, select option 3 and enter a search term. The application will create a FHIR search query using the search term and display the results.

## Next steps

We will be adding more features to this application in the future, to have a more extensive test of Firely SDK. If you have any suggestions, please feel free to open an issue.

## License

This project is licensed under the GNU General Public License - see the [LICENSE](LICENSE.txt) file for details