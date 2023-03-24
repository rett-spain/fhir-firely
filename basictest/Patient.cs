using System;

namespace FhirApp
{
    public class Patient
    {
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }

        public Patient(string firstName, string familyName, DateTime birthDate, string phoneNumber)
        {
            FirstName = firstName;
            FamilyName = familyName;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return $"Patient: {FirstName} {FamilyName} {BirthDate} {PhoneNumber}";
        }
    }
}