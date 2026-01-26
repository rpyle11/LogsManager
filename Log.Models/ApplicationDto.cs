using System.ComponentModel.DataAnnotations;

namespace Log.Models
{
    public class ApplicationDto
    {
        public int Id { get; init; }
        [Required(ErrorMessage = "required")]
        [Length(3, 50, ErrorMessage = "length is max: 50, min: 3 characters")]
        public string? AppName { get; set; }

        [Required(ErrorMessage = "required")]
        [Length(5, 50, ErrorMessage = "length is max: 50, min: 5 characters")]
        public string? AppDescription { get; set; }

        public DateTime DateCreated { get; init; } = DateTime.Now;

        public bool Active { get; set; } = true;

        public string? ActiveString { get; init; }

    }
}
