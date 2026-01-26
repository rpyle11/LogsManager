using Log.Models;
using LogApi.Entities;

namespace LogApi.Services;

public interface IRepositoryService
{
    Task<int> GetLogTypeId(string logType);

    Task<bool> WriteLog(AppLogDto logData, int lgType);
    Task<Applications?> AddUpdateApplication(Applications application);

    Task<List<Applications>?> GetAllApplications();

    Task<List<GetLogsView>?> GetLogs(GetLogsParameters parameters);

    Task<GetMinMaxDatesView?> GetMinMaxDates();

    Task<string> GetLogType(int logTypeId);
}