using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BandAidMySql.Models
{
    public partial class Reviews
    {
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Ocjena je obavezna")]
        [Range(1, 5)]
        [Display(Name = "Ocjena")]
        public int Rate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "dd.MM.yyyy HH:mm")]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int UserId { get; set; }

        public Users User { get; set; }
    }
}
