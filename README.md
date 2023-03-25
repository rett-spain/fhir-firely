# FHIR Console Application to test Firely .NET SDK

This is a simple .NET console application that allows users to interact with a FHIR server using the [Firely .NET SDK](https://docs.fire.ly)

## Requirements

To use this application, you will need:

* .NET Core 3.1 or later
* A FHIR server to connect to
* A Keyvault to store the FHIR server's credentials
  * azureAdInstance
  * tenantId
  * clientId
  * clientSecret
  * fhirScope

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

## License

This project is licensed under the GNU General Public License - see the [LICENSE](LICENSE.txt) file for details