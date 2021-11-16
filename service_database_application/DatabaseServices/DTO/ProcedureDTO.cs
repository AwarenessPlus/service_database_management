using System;

namespace DatabaseServices.DTO
{
    public class ProcedureDTO
    {
        private int procedureID;
        private int _medicID;
        private string _procedureName;
        private string _patientStatus;
        private int _asa;
        private int _patientID;
        private int _patientAge;
        private PatientDTO _patientInfo;
        private DateTime _procedureDate;

        public string ProcedureName { get => _procedureName; set => _procedureName = value; }
        public string PatientStatus { get => _patientStatus; set => _patientStatus = value; }
        public int Asa { get => _asa; set => _asa = value; }
        public int PatientID { get => _patientID; set => _patientID = value; }
        public int MedicID { get => _medicID; set => _medicID = value; }
        public int PatientAge { get => _patientAge; set => _patientAge = value; }
        public PatientDTO PatientInfo { get => _patientInfo; set => _patientInfo = value; }
        public DateTime ProcedureDate { get => _procedureDate; set => _procedureDate = value; }
        public int ProcedureID { get => procedureID; set => procedureID = value; }
    }
}
