namespace Dsw2026Ej15.Api.Models
{
    public class CreateDoctorRequest
    {
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber {  get; set; } = string.Empty;
        public Guid SpecialityId { get; set; }
    }
}
