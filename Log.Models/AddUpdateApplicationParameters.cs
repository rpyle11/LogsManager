using System.ComponentModel.DataAnnotations;

namespace Log.Models
{
    public class AddUpdateApplicationParameters
    {
        public int Id { get; init; }

        [Required(ErrorMessage = "AppName is required")]
        [Length(3, 50, ErrorMessage = "AppName length is max: 50, min: 3 characters")]
        public string? AppName { get; init; }

        [Required(ErrorMessage = "required")]
        [Length(5, 50, ErrorMessage = "length is max: 50, min: 5 characters")]
        public string? AppDescription { get; init; }

        public DateTime DateCreated { get; init; }
       
        public bool Active { get; init; }

    }
}
