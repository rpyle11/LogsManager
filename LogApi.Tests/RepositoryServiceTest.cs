using Log.Models;
using LogApi.Entities;
using LogApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LogApi.Tests
{
    public class RepositoryServiceTest
    {
        private static AppLogsContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppLogsContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return (AppLogsContext)Activator.CreateInstance(typeof(AppLogsContext), options)!;
        }

        [Fact]
        public async Task GetApplicationId_ReturnsId_WhenApplicationExists()
        {
            var dbName = Guid.NewGuid().ToString();
            await using (var seedContext = CreateContext(dbName))
            {
                seedContext.Applications.Add(new Applications
                {
                    Id = 42,
                    AppName = "MyApp",
                    AppDescription = "desc",
                    Active = true
                });
                await seedContext.SaveChangesAsync();
            }

            await using (var context = CreateContext(dbName))
            {
                var service = new RepositoryService(context);

                var method = typeof(RepositoryService).GetMethod("GetApplicationId",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.NotNull(method);

                var task = (Task<int>)method.Invoke(service, ["MyApp"])!;
                var result = await task;

                Assert.Equal(42, result);
            }
        }

        [Fact]
        public async Task GetApplicationId_ReturnsZero_WhenApplicationNotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var service = new RepositoryService(context);

            var method =
                typeof(RepositoryService).GetMethod("GetApplicationId", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);

            var task = (Task<int>)method.Invoke(service, ["NonExistentApp"])!;
            var result = await task;

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task WriteLog_ReturnsTrue_AndPersistsLog_WhenApplicationExists()
        {
            var dbName = Guid.NewGuid().ToString();
            // seed application
            await using (var seed = CreateContext(dbName))
            {
                seed.Applications.Add(new Applications
                {
                    Id = 5,
                    AppName = "MyApp",
                    AppDescription = "desc",
                    Active = true
                });
                await seed.SaveChangesAsync();
            }

            await using (var context = CreateContext(dbName))
            {
                var service = new RepositoryService(context);

                var dto = new AppLogDto
                {
                    AppName = "MyApp",
                    AppUser = "tester",
                    AppVersion = "1.0.0",
                    LogDate = DateTime.UtcNow,
                    LogMessage = "Unit test message",
                    SendEmailAddressList = "a@b.com"
                };

                var result = await service.WriteLog(dto, lgType: 2);

                Assert.True(result);

                // verify persisted
                var saved = await context.AppLogs.SingleAsync(a => a.LogMessage == "Unit test message");
                Assert.Equal("tester", saved.AppUser);
                Assert.Equal("1.0.0", saved.AppVersion);
                Assert.Equal(5, saved.AppId); // application id resolved
                Assert.Equal(2, saved.LogTypeId);
                Assert.Equal(dto.SendEmailAddressList, saved.SendEmailAddressList);
            }
        }

        [Fact]
        public async Task WriteLog_ReturnsTrue_AndPersistsLog_WithAppIdZero_WhenApplicationMissing()
        {
            var dbName = Guid.NewGuid().ToString();

            await using var context = CreateContext(dbName);
            var service = new RepositoryService(context);

            var dto = new AppLogDto
            {
                AppName = "NoSuchApp",
                AppUser = "tester2",
                AppVersion = "2.0.0",
                LogDate = DateTime.UtcNow,
                LogMessage = "Unit test message 2",
                SendEmailAddressList = "x@y.com"
            };

            var result = await service.WriteLog(dto, lgType: 3);

            Assert.True(result);

            var saved = await context.AppLogs.SingleAsync(a => a.LogMessage == "Unit test message 2");
            Assert.Equal("tester2", saved.AppUser);
            Assert.Equal("2.0.0", saved.AppVersion);
            Assert.Equal(0, saved.AppId); // no application found -> AppId 0
            Assert.Equal(3, saved.LogTypeId);
        }
    }

}
