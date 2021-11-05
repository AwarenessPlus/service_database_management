using System;

namespace DatabaseServices.DTO
{
    public class PatientDTO
    {
        private int patientID;
        private string _firstName;
        private string _lastName;
        private DateTime _birthDate;
        private string _bloodGroup;
        private string _rh;
        private string _sex;
        private int _documentID;
        private int _patientAge;

        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string BloodGroup { get => _bloodGroup; set => _bloodGroup = value; }
        public string Rh { get => _rh; set => _rh = value; }
        public string Sex { get => _sex; set => _sex = value; }
        public int DocumentID { get => _documentID; set => _documentID = value; }
        public int PatientID { get => patientID; set => patientID = value; }
        public DateTime BirthDate { get => _birthDate; set => _birthDate = value; }
        public int PatientAge { get => _patientAge; set => _patientAge = value; }
    }
}
