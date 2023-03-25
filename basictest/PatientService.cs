using System;
using System.Collections.Generic;
using System.Linq;

namespace FhirApp
{
    public class PatientService
    {
        private List<Patient> _patients = new List<Patient>();
        private FhirService _fhirService;

        public PatientService(FhirService fhirService)
        {
            _fhirService = fhirService;
        }

        public void CreatePatient(string firstName, string familyName, DateTime birthDate, string phoneNumber)
        {
            var patient = new Patient(firstName, familyName, birthDate, phoneNumber);
            _patients.Add(patient);
            // TODO: Send FHIR request to create patient on server
        }

        public List<Patient> GetAllPatients()
        {
            // TODO: Send FHIR request to get all patients from server
            return _patients;
        }

        public List<Patient> SearchPatients(string searchWord)
        {
            // TODO: Send FHIR search request to server
            return _patients.Where(p => p.FirstName.Contains(searchWord) || p.FamilyName.Contains(searchWord)).ToList();
        }
    }
}