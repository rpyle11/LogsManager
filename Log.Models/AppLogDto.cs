namespace Log.Models
{
    public class AppLogDto
    {
        public string? AppName { get; init; }


        public string? AppVersion { get; init; }


        public string? AppUser { get; init; }


        public DateTime LogDate { get; init; }


        public string? LogMessage { get; init; }


        public string? MessageType { get; set; }


        public string? FromAddress { get; init; }


        public string? SendEmailAddressList { get; init; }


        public string? EmailSubject { get; init; }

    }
}
