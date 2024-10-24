

using System;

namespace App.Core.Models.Patient
{
    public class PatientDto
    {
        public string PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }    
        public string ContactNumber { get; set; }        
        public string Email { get; set; }
        public string Address { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string BloodGroup { get; set; }
        public bool? IsChecked { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
