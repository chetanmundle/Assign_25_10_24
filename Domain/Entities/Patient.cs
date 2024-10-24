using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Patient
    {
        [Key]
        public string PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth {  get; set; }
        public string Gender { get; set; }
        [StringLength(20)]
        public string ContactNumber { get; set; }
        [StringLength(100)]
        public string Email {  get; set; }
        [StringLength(200)]
        public string Address { get; set; }

        [ForeignKey("Country")]
        public int? CountryId { get; set; }
        [ForeignKey("State")]
        public int? StateId { get; set; }
        [ForeignKey("City")]
        public int? CityId { get; set; }
        [StringLength(30)]
        public string BloodGroup { get; set; }
        public bool? IsChecked { get; set; }

        [ForeignKey("User")]
        public int? CreatedBy { get; set; }   // UserId

        public bool? IsDeleted { get; set; }

        public Country? Country { get; set; }
        public State? State { get; set; }
        public City? City { get; set; }
        public User? User { get; set; }
    }
}
