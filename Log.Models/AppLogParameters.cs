using System.ComponentModel.DataAnnotations;

namespace Log.Models
{
    public class AppLogParameters(string appName, string appVersion, string appUser, DateTime? logDate, string logMessage, string messageType, string fromAddress, string sendEmailAddressList, string emailSubject)
    {
        [Required(ErrorMessage = "AppName is required")]
        [Length(3, 50, ErrorMessage = "AppName length is max: 50, min: 3 characters")]
        public string AppName { get; } = appName;

        [Required(ErrorMessage = "AppVersion is required")]
        [Length(5, 50, ErrorMessage = "AppVersion length is max: 50, min: 5 characters")]
        public string AppVersion { get; } = appVersion;

        [Required(ErrorMessage = "AppUser is required")]
        [Length(3, 20, ErrorMessage = "AppUser length is max: 50, min: 3 characters")]
        public string AppUser { get; } = appUser;

        [Required(ErrorMessage = "LogDate is required")]
        public DateTime? LogDate { get; } = logDate;

        [Required(ErrorMessage = "LogMessage is required")]
        [Length(3, 8000, ErrorMessage = "LogMessage length is max: 8000, min: 3 characters")]
        public string LogMessage { get; } = logMessage;

        [Required(ErrorMessage = "MessageType is required")]
        [AllowedValues("Error", "Message")]
        public string MessageType { get; } = messageType;

        [Length(5, 100, ErrorMessage = "FromAddress length is max: 50, min: 5 characters")]
        public string FromAddress { get; } = fromAddress;

        [Length(5, 1000, ErrorMessage = "ToAddress length is max: 50, min: 5 characters")]
        public string SendEmailAddressList { get; } = sendEmailAddressList;

        [Length(3,50, ErrorMessage = "EmailSubject length is max: 50, min: 3 characters")]
        public string EmailSubject { get; } = emailSubject;
    }
}
