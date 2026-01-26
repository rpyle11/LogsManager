using AutoMapper;
using Log.Models;
using LogApi.Entities;
using LogApi.Models;
using Microsoft.Extensions.Options;

namespace LogApi.Services
{
    public class LogDataService(IRepositoryService repositoryService, IEmailService emailService, IMapper mapper, IOptions<AppSettings> settings) : ILogDataService
    {
        public Task<int> ValidateMessageType(AppLogParameters parameters)
        {
            return repositoryService.GetLogTypeId(parameters.MessageType);
        }

        public async Task<bool> LogMessage(AppLogParameters parameters, int lgType)
        {

            var log = new AppLogDto
            {
                AppName = parameters.AppName,
                AppUser = parameters.AppUser,
                AppVersion = parameters.AppVersion,
                EmailSubject = parameters.EmailSubject,
                FromAddress = parameters.FromAddress,
                LogDate = parameters.LogDate ?? DateTime.Now,
                LogMessage = parameters.LogMessage,
                MessageType = parameters.MessageType,
                SendEmailAddressList = parameters.SendEmailAddressList
            };

            var writeToDb = await repositoryService.WriteLog(log, lgType);

            if (string.IsNullOrEmpty(log.SendEmailAddressList)) return writeToDb;

            var emFields = new EmailFields
            {
                ToAddress = log.SendEmailAddressList,
                UseHtml = true,
                Subject = log.EmailSubject,
                MessageBody = log.LogMessage,
                FromAddress = log.FromAddress
            };

            var emSent = await emailService.SendMessage(emFields);

            return emSent && writeToDb;


        }

        public async Task<bool> LogEventMessage(EventViewParameters parameters)
        {
            var log = new AppLogDto
            {
                AppName = parameters.AppName,
                AppUser = parameters.User,
                EmailSubject = string.IsNullOrEmpty(parameters.EventSubject) ? $"{parameters.AppName} Alert" : parameters.EventSubject,
                FromAddress = settings.Value.FromAddr,
                LogDate = DateTime.Now,
                LogMessage = parameters.EventMsg,
                MessageType = await repositoryService.GetLogType(parameters.EventTypeId),
                SendEmailAddressList = parameters.EmailToAddr
            };



            var writeToDb = await repositoryService.WriteLog(log, parameters.EventTypeId);

            if (string.IsNullOrEmpty(log.SendEmailAddressList)) return writeToDb;

            var emFields = new EmailFields
            {
                ToAddress = log.SendEmailAddressList,
                UseHtml = parameters.SendEmailAsHtml,
                Subject = log.EmailSubject,
                MessageBody = log.LogMessage,
                FromAddress = log.FromAddress
            };

            var emSent = await emailService.SendMessage(emFields);

            return emSent && writeToDb;
        }

        public async Task<ApplicationDto?> AddUpdateApplication(AddUpdateApplicationParameters parameters)
        {

            var application = mapper.Map<Applications>(parameters);

            var data = await repositoryService.AddUpdateApplication(application);

            return data != null ? mapper.Map<ApplicationDto>(data) : null;
        }

        public async Task<List<ApplicationDto>?> GetAllApplications()
        {

            var data = await repositoryService.GetAllApplications();

            return data != null ? mapper.Map<List<ApplicationDto>>(data) : null;

        }

        public async Task<List<LogsDto>?> GetLogs(GetLogsParameters parameters)
        {
            var data = await repositoryService.GetLogs(parameters);

            return data != null ? mapper.Map<List<LogsDto>>(data) : null;
        }

        public async Task<MinMaxDatesDto?> GetMinMaxDates()
        {

            var data = await repositoryService.GetMinMaxDates();

            return data != null ? mapper.Map<MinMaxDatesDto>(data) : null;
        }

        public Task<bool> SendEmail(EmailFields parameters)
        {
            return emailService.SendMessage(parameters);
        }
    }
}
