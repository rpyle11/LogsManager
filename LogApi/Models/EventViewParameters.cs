using System.ComponentModel.DataAnnotations;

namespace LogApi.Models
{
    public class EventViewParameters
    {
        [Required]
        [Length(5, 2000)]
        public string? EventMsg { get; set; }

        [Required]
        public int EventTypeId { get; set; }

        [Required]
        [Length(3, 50)]
        public string? AppName { get; set; }

        [Required]
        [Length(3, 20)]
        public string? User { get; set; }

        [Required]
        [Length(3, 1000)]
        public string? EmailToAddr { get; set; }

        public bool SendEmailAsHtml { get; set; }

        [Length(5, 50)]
        public string? EventSubject { get; set; }
    }
}
