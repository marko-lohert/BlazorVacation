using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorVacation.Shared
{
    public class Vacation
    {
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime TillDate { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage="The maximum length of the note is 50 characters.")]
        public string? Note { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public bool Approved { get; set; }
        [Required]
        public bool SetUpOutOfOfficeEmail { get; set; }
    }
}
