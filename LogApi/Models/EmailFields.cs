using System.ComponentModel.DataAnnotations;

namespace LogApi.Models
{
    public class EmailFields
    {
        [Required]
        [Length(3, 50)]
        public string? Subject { get; init; }

        [Required]
        [Length(5, 1000)]
        public string? MessageBody { get; init; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string? FromAddress { get; init; }

        [Required]
        public string? ToAddress { get; init; }

        [Required]
        public bool UseHtml { get; init; }
    }
}
