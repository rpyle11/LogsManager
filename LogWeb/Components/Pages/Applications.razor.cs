using Log.Models;
using LogWeb.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Reflection;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace LogWeb.Components.Pages
{
    public partial class Applications
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        [CascadingParameter]
        public DialogFactory? DialogAlert { get; set; }

        [Inject] private ILogService? LogService { get; set; }

        [Inject] private NavigationManager? NavManager { get; set; }

       
        private List<ApplicationDto>? ApplicationData { get; set; }

        private IEnumerable<ApplicationDto> SelectedApplications { get; set; } = [];

        private ApplicationDto? SelectedApplication { get; set; } = new();

        private string? AppUser { get; set; }

        private bool PageWait { get; set; }

        private bool ShowDetails { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            AppUser = authState.User.Identity?.Name?.Split('\\').Last();

            await LoadApplications();
        }

        private async Task LoadApplications()
        {
            try
            {
                PageWait = true;
                var data = await LogService?.GetAllApplications()!;

                if (data != null)
                {
                    ApplicationData = data.OrderBy(o => o.AppName).ToList();
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

        private void CancelDetails()
        {
            ShowDetails = false;
            SelectedApplication = null;
            SelectedApplications = [];
            StateHasChanged();

        }

        private async Task RowClick(GridRowClickEventArgs args)
        {

            try
            {

                ShowDetails = true;

                SelectedApplication = (ApplicationDto)args.Item;

                if (ApplicationData != null)
                    SelectedApplications = ApplicationData.Where(w => w.Id == SelectedApplication.Id);

                StateHasChanged();

            }

            catch (Exception ex)
            {
                await LogService?.LogAlert(AppLogPrep.AppLogSetup(AppUser, NavManager?.Uri,
                    MethodName.GetMethodName(MethodBase.GetCurrentMethod()), ex))!;

                NavManager?.NavigateTo("./error");


            }

        }

        private void AddApplication()
        {
            SelectedApplication = new ApplicationDto { DateCreated = DateTime.Now };
            SelectedApplications = [];
            ShowDetails = true;

            StateHasChanged();


        }

        private void EditApplication(GridCommandEventArgs args)
        {
            SelectedApplication = args.Item as ApplicationDto;
            if (ApplicationData != null)
                SelectedApplications =
                    ApplicationData.Where(w => SelectedApplication != null && w.Id == SelectedApplication.Id);
            ShowDetails = true;
            StateHasChanged();
        }

        private async void SaveApplication()
        {
            try
            {
                PageWait = true;

                if (SelectedApplication != null)
                {
                    var added = await LogService?.AddUpdateApplication(new AddUpdateApplicationParameters
                    {
                        Active = SelectedApplication.Active,
                        AppName = SelectedApplication.AppName,
                        AppDescription = SelectedApplication.AppDescription,
                        Id = SelectedApplication.Id,
                        DateCreated = SelectedApplication.Id == 0 ? DateTime.Now : SelectedApplication.DateCreated
                    })!;

                    if (added != null)
                    {
                        await LoadApplications();
                        if (ApplicationData != null)
                            SelectedApplications = ApplicationData.Where(w => w.Id == added.Id);
                        await DialogAlert?.AlertAsync("Application Successfully Saved", "Save")!;
                    }
                    else
                    {
                        SelectedApplications = [];
                        await DialogAlert?.AlertAsync("Application Not Saved", "Error")!;
                    }
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
    }
}
