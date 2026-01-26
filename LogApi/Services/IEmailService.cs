using LogApi.Models;

namespace LogApi.Services;

public interface IEmailService
{
    Task<bool> SendMessage(EmailFields emData);
}