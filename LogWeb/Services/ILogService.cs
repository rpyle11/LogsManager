using Log.Models;

namespace LogWeb.Services;

public interface ILogService
{
    Task<bool> LogAlert(AppLog appLog);

    Task<List<LogsDto>?> GetLogs(GetLogsParameters parameters);

    Task<MinMaxDatesDto?> GetMinMaxDates();

    Task<List<ApplicationDto>?> GetAllApplications();

    Task<ApplicationDto?> AddUpdateApplication(AddUpdateApplicationParameters parameters);
}