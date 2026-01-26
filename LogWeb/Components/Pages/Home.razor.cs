using Log.Models;
using LogWeb.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Reflection;

namespace LogWeb.Components.Pages
{
    public partial class Home
    {
        [CascadingParameter]
        private Task<AuthenticationState>? AuthenticationStateTask { get; set; }

        [Inject] private ILogService? LogService { get; set; }

        [Inject] private NavigationManager? NavManager { get; set; }

       // [PersistentState(AllowUpdates = true, RestoreBehavior = RestoreBehavior.SkipLastSnapshot)]
        private List<LogsDto>? LogData { get; set; }

        private DateTime StartDate { get; set; } = DateTime.Now;

        private DateTime EndDate { get; set; } = DateTime.Now;

        private DateTime MinDate { get; set; }

        private DateTime MaxDate { get; set; }

        private string? AppUser { get; set; }

        private bool PageWait { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationStateTask != null)
            {
                var authState = await AuthenticationStateTask;
                AppUser = authState.User.Identity?.Name?.Split('\\').Last();


                await LoadInitialDates();

                await LoadLogs();


            }


        }

        private async Task LoadInitialDates()
        {
            try
            {

                var data = await LogService?.GetMinMaxDates()!;

                if (data == null) return;
                MinDate = data.MinDate.AddDays(-1);
                MaxDate = data.MaxDate;


                EndDate = MaxDate;
                StartDate = MaxDate.AddDays(-30);



            }
            catch (Exception ex)
            {
                await LogService?.LogAlert(AppLogPrep.AppLogSetup(AppUser, NavManager?.Uri,
                    MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;

                NavManager?.NavigateTo("./error");

            }
        }

        private async Task LoadLogs()
        {
            try
            {

                PageWait = true;
                var data = await LogService?.GetLogs(new GetLogsParameters
                {
                    EndDate = EndDate,
                    StartDate = StartDate
                })!;

                if (data != null)
                {
                    LogData = [.. data.OrderByDescending(o => o.LogDate)];
                }

                PageWait = false;
                StateHasChanged();

            }
            catch (Exception ex)
            {
                await LogService?.LogAlert(AppLogPrep.AppLogSetup(AppUser, NavManager?.Uri,
                    MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;

                NavManager?.NavigateTo("./error");
            }
        }

        private async void StartDateChanged(DateTime dtDate)
        {
            try
            {
                StartDate = dtDate;
                await LoadLogs();
            }
            catch (Exception ex)
            {
                await LogService?.LogAlert(AppLogPrep.AppLogSetup(AppUser, NavManager?.Uri,
                    MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;

                NavManager?.NavigateTo("./error");
            }


        }

        private async void EndDateChanged(DateTime dtDate)
        {
            try
            {
                EndDate = dtDate;
                await LoadLogs();
            }
            catch (Exception ex)
            {
                await LogService?.LogAlert(AppLogPrep.AppLogSetup(AppUser, NavManager?.Uri,
                    MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;

                NavManager?.NavigateTo("./error");
            }

        }
    }
}
