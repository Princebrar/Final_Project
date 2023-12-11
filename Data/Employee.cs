using System;

namespace Final_Project.Data
{
    // Abstract class representing a person
    public abstract class Person
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public DateOnly dob { get; set; }
        public string gender { get; set; }
        public string address { get; set; }

        // Abstract method to be implemented by derived classes
        public abstract string GetFullName();
    }

    // Interface representing contact information
    public interface IContactInfo
    {
        string phone { get; set; }
        string GetContactDetails();
    }

    // Employee class inheriting from Person and implementing IContactInfo
    public class Employee : Person, IContactInfo
    {
        public int employeeID { get; set; }
        public string job { get; set; }
        public string phone { get; set; }

        public override string ToString()
        {
            return $"{employeeID}, {GetFullName()}, {dob}, {phone}, {gender}, {job}, {address}";
        }

        // Implementation of the abstract method from Person
        public override string GetFullName()
        {
            return $"{first_name} {last_name}";
        }

        // Implementation of the interface method from IContactInfo
        public string GetContactDetails()
        {
            return $"Phone: {phone}, Address: {address}";
        }
    }
}
