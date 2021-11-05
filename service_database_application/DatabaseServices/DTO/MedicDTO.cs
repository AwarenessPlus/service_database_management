using System;

namespace DatabaseServices.DTO
{
    public class MedicDTO
    {
        private int _medicID;
        private string _firstName;
        private string _username;
        private string _lastName;
        private DateTime _birthDate;
        private int _age;
        private int _semester;
        private string _rotation;

        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public int Age { get => _age; set => _age = value; }
        public int Semester { get => _semester; set => _semester = value; }
        public string Rotation { get => _rotation; set => _rotation = value; }
        public string Username { get => _username; set => _username = value; }
        public int MedicID { get => _medicID; set => _medicID = value; }
        public DateTime BirthDate { get => _birthDate; set => _birthDate = value; }
    }
}
