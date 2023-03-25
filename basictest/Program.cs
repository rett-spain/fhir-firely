using Hl7.Fhir.Model;

namespace FhirApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a new FHIR service
            Console.WriteLine("Connecting with FHIR Service...");
            var fhirService = new FhirService();

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
        }
    }
}