using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BandAidMySql.Models
{
    public partial class Users
    {
        public Users()
        {
            Events = new HashSet<Events>();
            Reviews = new HashSet<Reviews>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Display(Name = "E-mail")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail je obavezan")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Ime je obavezno")]
        public string Name { get; set; }

        [Display(Name = "Kontakt broj")]
        public string PhoneNumber { get; set; }

        public string Street { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Zaporka je obavezna")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Zaporka mora sadržavati minimalno 6 znakova")]
        public string PassHash { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ProfileImg { get; set; }

        [Required(ErrorMessage = "Odaberite ulogu")]
        public int RoleId { get; set; }
        public int IsEmailVerified { get; set; }
        public string ActivationCode { get; set; }
        public string Youtube { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Userroles Role { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Events> Events { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Reviews> Reviews { get; set; }
    }
}
