using Log.Models;
using LogApi.Models;

namespace LogApi.Services;

public interface ILogDataService
{
    Task<int> ValidateMessageType(AppLogParameters parameters);
    Task<bool> LogMessage(AppLogParameters parameters, int lgType);
    Task<ApplicationDto?> AddUpdateApplication(AddUpdateApplicationParameters parameters);
    Task<List<ApplicationDto>?> GetAllApplications();

    Task<List<LogsDto>?> GetLogs(GetLogsParameters parameters);

    Task<MinMaxDatesDto?> GetMinMaxDates();

    Task<bool> SendEmail(EmailFields parameters);

    Task<bool> LogEventMessage(EventViewParameters parameters);
}