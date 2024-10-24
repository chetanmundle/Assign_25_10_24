using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool? IsChecked { get; set; }

        public string LastFirstPass { get; set; }
        public string LastSecondPass { get; set; }
        public string LastThirdPass { get; set; }

        public int? PatientCreated {  get; set; }
    }
}
