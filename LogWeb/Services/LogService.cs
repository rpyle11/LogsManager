using Log.Models;
using LogWeb.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace LogWeb.Services
{
    public class LogService(HttpClient httpClient, IOptions<AppSettings> settings) : ILogService
    {
        public Task<bool> LogAlert(AppLog appLog)
        {
            var log = new AppLogDto
            {
                AppName = "LogsManager.Web",
                AppUser = appLog.AppUser,
                AppVersion = typeof(LogService).Assembly.GetName().Version?.ToString(),
                EmailSubject = settings.Value.LogEmailSubject,
                FromAddress = settings.Value.LogFromAddress,
                LogDate = DateTime.Now,
                LogMessage = appLog.LogMsg,
                MessageType = appLog.MessageType.ToString(),
                SendEmailAddressList = appLog.SendEmail ? settings.Value.LogToAddress : string.Empty,
            };
            return SendLog(log);
        }

        public async Task<List<LogsDto>?> GetLogs(GetLogsParameters parameters)
        {
            httpClient.DefaultRequestHeaders.Clear();
            var response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}/ui/logs", parameters);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<List<LogsDto>>();
            }

            return null;
        }

        public async Task<MinMaxDatesDto?> GetMinMaxDates()
        {

            httpClient.DefaultRequestHeaders.Clear();


            var response = await httpClient.GetAsync($"{httpClient.BaseAddress}/ui/minmaxdates");


            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<MinMaxDatesDto>();
            }

            return null;
        }

        public async Task<List<ApplicationDto>?> GetAllApplications()
        {

            httpClient.DefaultRequestHeaders.Clear();


            var response = await httpClient.GetAsync($"{httpClient.BaseAddress}/ui/applications");


            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<List<ApplicationDto>>();
            }

            return null;
        }

        public async Task<ApplicationDto?> AddUpdateApplication(AddUpdateApplicationParameters parameters)
        {
            httpClient.DefaultRequestHeaders.Clear();
            var response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}/ui/addupdateapplication", parameters);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<ApplicationDto>();
            }

            return null;
        }

        private async Task<bool> SendLog(AppLogDto log)
        {

            httpClient.DefaultRequestHeaders.Clear();
            var response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}/log/setlog", log);
            return response.IsSuccessStatusCode;

        }
    }
}
