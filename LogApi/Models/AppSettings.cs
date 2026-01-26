namespace LogApi.Models
{
    public class AppSettings
    {
        public string? SmtpServer { get; init; }

        public string? EmailAddrSep { get; init; }

        public string? EmailRegExp { get; init; }
        public string? FromAddr { get; init; }
        public bool EmailAuthenticate { get; init; }

        public int EmailPort { get; init; }
        public string? EmailUsername { get; init; }
        public string? EmailPassword { get; init; }

        public bool IsLocal { get; init; }
    }
}
