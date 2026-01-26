using Log.Models;
using LogWeb.Models;
using LogWeb.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LogWeb.Tests
{
    public class LogServiceTests
    {
        private sealed class FakeHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
            : HttpMessageHandler
        {
            private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder = responder ?? throw new ArgumentNullException(nameof(responder));

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_responder(request));
            }
        }

        private static LogService CreateService(Func<HttpRequestMessage, HttpResponseMessage> responder)
        {
            var handler = new FakeHttpMessageHandler(responder);
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var settings = Options.Create(new AppSettings
            {
                LogEmailSubject = "sub",
                LogFromAddress = "from@x.com",
                LogToAddress = "to@x.com"
            });

            return new LogService(httpClient, settings);
        }

        [Fact]
        public async Task GetLogs_ReturnsList_WhenResponseIsOk()
        {
            // Arrange
            var expected = new List<LogsDto>
            {
                new LogsDto { AppName = "A", AppUser = "u1", LogMessage = "m1", LogDate = DateTime.UtcNow },
                new LogsDto { AppName = "B", AppUser = "u2", LogMessage = "m2", LogDate = DateTime.UtcNow }
            };

            var json = JsonSerializer.Serialize(expected);

            var service = CreateService(_ =>
            {
                // optional: assert request content / method if desired
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                return response;
            });

            var parameters = new GetLogsParameters
            {
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow
            };

            // Act
            var result = await service.GetLogs(parameters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Count, result.Count);
            Assert.Equal(expected[0].AppName, result[0].AppName);
            Assert.Equal(expected[1].LogMessage, result[1].LogMessage);
        }

        [Fact]
        public async Task GetLogs_ReturnsNull_WhenResponseIsNotOk()
        {
            // Arrange
            var service = CreateService(_ => new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(string.Empty)
            });

            var parameters = new GetLogsParameters
            {
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow
            };

            // Act
            var result = await service.GetLogs(parameters);

            // Assert
            Assert.Null(result);
        }

    }
}
