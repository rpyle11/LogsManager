using System.ComponentModel.DataAnnotations;

namespace LogWeb.Models
{
    public class AppSettings
    {
        public bool IsLocal { get; init; }
       

        [Required]
        [Length(3, 500)]
        public string? LogEmailSubject { get; init; }

        [Required]
        [Length(3, 150)]
        public string? LogFromAddress { get; init; }

        [Required]
        [Length(3, 1000)]
        public string? LogToAddress { get; init; }
    }
}
