using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace FhirApp
{
    public class PatientService
    {
        private FhirService _fhirService;

        public PatientService(FhirService fhirService)
        {
            _fhirService = fhirService;
        }

        public Patient CreatePatient(string firstName, string middleName, string familyName, string birthDate)
        {
            // Create a new patient
            var newPatient = new Patient()
            {
                Name = new List<HumanName>()
                {
                    new HumanName()
                    {
                        Given = new List<string> { firstName, middleName },
                        Family = familyName
                    }
                },
                Gender = AdministrativeGender.Male,
                BirthDate = birthDate,
                Identifier = new List<Identifier>()
                {
                    new Identifier()
                    {
                        System = "http://acme.org/mrns",
                        Value = "12345"
                    }
                }
            };

            // Send FHIR request to create patient on server
            var response = _fhirService.Execute(client => client.Create(newPatient));

            if (response is Patient patient)
            {
                return patient;
            }
            else
            {
                throw new Exception("Failed to create patient");
            }
        }

        // Search for patients given a search name
        public Bundle SearchName(string searchName)
        {
            var response = _fhirService.Execute(client => client.Search("Patient", new string[] { $"name={searchName}" }));

            if (response is Bundle bundle)
            {
                return bundle;
            }
            else
            {
                throw new Exception("Failed to search for patients");
            }
        }

        // Get all patients
        public List<Patient> GetAllPatients()
        {
            var searchResult = _fhirService.Execute(client => client.Search<Patient>());

            var patients = searchResult.Entry
                .Where(entry => entry.Resource is Patient)
                .Select(entry => entry.Resource as Patient)
                .ToList();

            return patients;
        }
    }
}