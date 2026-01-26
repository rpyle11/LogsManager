namespace Log.Models
{
    public class LogsDto
    {
        public int Id { get; init; }
        public int AppId { get; init; }
        public string? AppVersion { get; init; }
        public string? AppUser { get; init; }
        public DateTime LogDate { get; init; }
        public string? LogMessage { get; init; }
        public int LogTypeId { get; init; }
        public string? SendEmailAddressList { get; init; }
        public string? AppName { get; init; }
        public string? LogType { get; init; }

    }
}
