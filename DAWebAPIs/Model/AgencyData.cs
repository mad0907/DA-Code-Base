using System;
namespace DAWebAPIs.Model
{
	public class AgencyData
	{
        public Guid id { get; set; }
        public string? PatientName { get; set; }
        public string? StartOfCare { get; set; }
        public string? StartOfEpisode { get; set; }
        public string? EndOfEpisode { get; set; }
        public string? EpisodeStatus { get; set; }
        public string? MedicalRecordNo { get; set; }
        public string? ServiceLine { get; set; }
        public string? PatientAddress { get; set; }
        public string? Zip { get; set; }
        public string? PayorSource { get; set; }
        public string? PhysicianNPI { get; set; }
        public string? PhysicianName { get; set; }
        public string? PGEHRId { get; set; }
        public string? AgencyType { get; set; }
        public string? AgencyEHRId { get; set; }
        public string? BillingProvider { get; set; }
        public string? NPI { get; set; }
        public string? FirstDiagnosis { get; set; }
        public string? SecondDiagnosis { get; set; }
        public string? ThirdDiagnosis { get; set; }
        public string? FourthDiagnosis { get; set; }
        public string? FifthDiagnosis { get; set; }
        public string? SixthDiagnosis { get; set; }
        public string? Line1DOSFrom { get; set; }
        public string? Line1DOSTo { get; set; }
        public string? Line1POS { get; set; }
        public string? SupervisingProvider { get; set; }
        public string? PhysicianPhone { get; set; }
        public string? PhysicianAddress { get; set; }
        public string? CityStateZip { get; set; }
        public string? NumberOfEpisodes { get; set; }
        public string? Status485 { get; set; }
        public string? F2fStatus { get; set; }
        public string? NumberOfDocuments { get; set; }
        public string? InsuranceCompanyName { get; set; }
        public string? InsuranceID { get; set; }
        public string? Agency { get; set; }
        public string? PatientAccountNo { get; set; }
        public string? PatientSex { get; set; }
        public string? PatientCity { get; set; }
        public string? PatientState { get; set; }
        public string? PatientZip { get; set; }
        public string? Line1CPT { get; set; }
        public string? Line1Units { get; set; }
        public string? Line1charges { get; set; }
        public string? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class AgencyDataDto
    {
        public string? PatientName { get; set; }
        public string? StartOfCare { get; set; }
        public string? StartOfEpisode { get; set; }
        public string? EndOfEpisode { get; set; }
        public string? EpisodeStatus { get; set; }
        public string? MedicalRecordNo { get; set; }
        public string? ServiceLine { get; set; }
        public string? PatientAddress { get; set; }
        public string? Zip { get; set; }
        public string? PayorSource { get; set; }
        public string? PhysicianNPI { get; set; }
        public string? PhysicianName { get; set; }
        public string? PGEHRId { get; set; }
        public string? AgencyType { get; set; }
        public string? AgencyEHRId { get; set; }
        public string? BillingProvider { get; set; }
        public string? NPI { get; set; }
        public string? FirstDiagnosis { get; set; }
        public string? SecondDiagnosis { get; set; }
        public string? ThirdDiagnosis { get; set; }
        public string? FourthDiagnosis { get; set; }
        public string? FifthDiagnosis { get; set; }
        public string? SixthDiagnosis { get; set; }
        public string? Line1DOSFrom { get; set; }
        public string? Line1DOSTo { get; set; }
        public string? Line1POS { get; set; }
        public string? SupervisingProvider { get; set; }
        public string? PhysicianPhone { get; set; }
        public string? PhysicianAddress { get; set; }
        public string? CityStateZip { get; set; }
        public string? NumberOfEpisodes { get; set; }
        public string? Status485 { get; set; }
        public string? F2fStatus { get; set; }
        public string? NumberOfDocuments { get; set; }
        public string? InsuranceCompanyName { get; set; }
        public string? InsuranceID { get; set; }
        public string? Agency { get; set; }
        public string? PatientAccountNo { get; set; }
        public string? PatientSex { get; set; }
        public string? PatientCity { get; set; }
        public string? PatientState { get; set; }
        public string? PatientZip { get; set; }
        public string? Line1CPT { get; set; }
        public string? Line1Units { get; set; }
        public string? Line1charges { get; set; }
        public string? CreatedBy { get; set; }
    }
}

