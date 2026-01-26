using LogApi.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
#pragma warning disable CA1416

namespace LogApi.Services
{
    public class EmailService(IOptions<AppSettings> settings) : IEmailService
    {
        public async Task<bool> SendMessage(EmailFields emData)
        {
            try
            {
                var message = new MailMessage();
                var client = new SmtpClient();

                //set sender's address
                if (!string.IsNullOrEmpty(emData.FromAddress))
                {
                    message.From = VerifyEmailAddress(emData.FromAddress) ? new MailAddress(emData.FromAddress) : new MailAddress(settings.Value.FromAddr!);
                }
                else
                {
                    message.From = new MailAddress(settings.Value.FromAddr!);
                }

                //allow multiple "to" address
                foreach (var address in emData.ToAddress?.Split(char.Parse(settings.Value.EmailAddrSep!))!)
                {
                    if (VerifyEmailAddress(address))
                        message.To.Add(new MailAddress(address));
                    else
                        throw new FormatException("Invalid email address");
                }

                message.IsBodyHtml = emData.UseHtml;
                message.Subject = emData.Subject;
                message.Body = emData.MessageBody;
                client.Host = settings.Value.SmtpServer!;

                if (settings.Value.IsLocal)
                {

                    client.Port = settings.Value.EmailPort;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(settings.Value.EmailUsername, settings.Value.EmailPassword);
                }
                else
                {
                    if (settings.Value.EmailAuthenticate)
                        client.Credentials = CredentialCache.DefaultNetworkCredentials;
                }

                await client.SendMailAsync(message);

                message.Dispose();
                client.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogsManager", $"{ex.GetType().Name}, Error in {MethodBase.GetCurrentMethod()?.Name}, Error: {ex.Message}");

            }

            return false;
        }

        private bool VerifyEmailAddress(string emailAddress)
        {
            if (emailAddress.Length <= 0) return false;
            var checkMatch = Regex.Match(emailAddress, settings.Value.EmailRegExp!);

            return checkMatch.Success;

        }
    }
}
