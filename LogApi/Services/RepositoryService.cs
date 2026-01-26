using Log.Models;
using LogApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
#pragma warning disable CA1416

namespace LogApi.Services
{
    public class RepositoryService(AppLogsContext context) : IRepositoryService
    {
        private async Task<int> GetApplicationId(string? name)
        {
            try
            {
                var id = await context.Applications.FirstOrDefaultAsync(f => f.AppName == name);

                if (id != null)
                    return id.Id;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogsManager", $"{ex.GetType().Name}, Error in {MethodBase.GetCurrentMethod()?.Name}, Error: {ex.Message}");

            }

            return 0;
        }

        public async Task<int> GetLogTypeId(string logType)
        {
            try
            {
                var id = await context.LogTypes.FirstOrDefaultAsync(f => f.LogType == logType);

                if (id != null)
                    return id.Id;

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogsManager", $"{ex.GetType().Name}, Error in {MethodBase.GetCurrentMethod()?.Name}, Error: {ex.Message}");

            }

            return 0;
        }

        public async Task<string> GetLogType(int logTypeId)
        {
            try
            {
                var logType = await context.LogTypes.FirstOrDefaultAsync(f => f.Id == logTypeId);

                if (logType != null)
                    return logType.LogType;

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogsManager", $"{ex.GetType().Name}, Error in {MethodBase.GetCurrentMethod()?.Name}, Error: {ex.Message}");

            }

            return string.Empty;
        }

        public async Task<bool> WriteLog(AppLogDto logData, int lgType)
        {
            try
            {


                var appLog = new AppLogs
                {
                    AppUser = logData.AppUser,
                    AppVersion = logData.AppVersion,
                    LogDate = logData.LogDate,
                    LogMessage = logData.LogMessage,
                    SendEmailAddressList = logData.SendEmailAddressList,
                    AppId = await GetApplicationId(logData.AppName),
                    LogTypeId = lgType
                };

                await context.AppLogs.AddAsync(appLog);
                var savedCount = await context.SaveChangesAsync();

                if (savedCount == 1)
                    return true;


            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogsManager", $"{ex.GetType().Name}, Error in {MethodBase.GetCurrentMethod()?.Name}, Error: {ex.Message}");

            }


            return false;
        }

        public async Task<Applications?> AddUpdateApplication(Applications application)
        {
            try
            {
                if (application.Id == 0)
                {
                    await context.Applications.AddAsync(application);
                    await context.SaveChangesAsync();

                    return application;
                }

                var inDb = await context.Applications.FirstOrDefaultAsync(f => f.Id == application.Id);
                if (inDb != null)
                {
                    inDb.Active = application.Active;
                    inDb.AppDescription = application.AppDescription;
                    inDb.AppName = application.AppName;


                    context.Applications.Update(inDb);
                    await context.SaveChangesAsync();

                    return inDb;
                }
            }

            catch (Exception ex)
            {
                EventLog.WriteEntry("LogsManager", $"{ex.GetType().Name}, Error in {MethodBase.GetCurrentMethod()?.Name}, Error: {ex.Message}");

            }

            return null;
        }

        public async Task<List<Applications>?> GetAllApplications()
        {
            try
            {
                return await context.Applications.ToListAsync();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogsManager", $"{ex.GetType().Name}, Error in {MethodBase.GetCurrentMethod()?.Name}, Error: {ex.Message}");
            }

            return null;

        }

        public async Task<List<GetLogsView>?> GetLogs(GetLogsParameters parameters)
        {
            try
            {
                return await context.GetLogsView
                    .Where(w => w.LogDate >= parameters.StartDate && w.LogDate <= parameters.EndDate).ToListAsync();


            }
            catch (Exception ex)
            {

                EventLog.WriteEntry("LogsManager", $"{ex.GetType().Name}, Error in {MethodBase.GetCurrentMethod()?.Name}, Error: {ex.Message}");


            }

            return null;
        }

        public async Task<GetMinMaxDatesView?> GetMinMaxDates()
        {
            try
            {
                var data = await context.GetMinMaxDatesView.FirstOrDefaultAsync();

                if (data != null)
                    return data;


            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogsManager", $"{ex.GetType().Name}, Error in {MethodBase.GetCurrentMethod()?.Name}, Error: {ex.Message}");

            }

            return null;
        }
    }
}
