using LogWeb.Models;
using LogWeb.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;

#pragma warning disable CA1416

namespace LogWeb.Components
{
    public partial class UserInfo
    {
        [CascadingParameter]
        private Task<AuthenticationState>? AuthenticationStateTask { get; set; }

        [Inject] private IOptions<AppSettings>? Settings { get; set; }

        private string? FullName { get; set; } = "Unknown";

        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationStateTask != null)
            {
                var authState = await AuthenticationStateTask;
                var user = authState.User.Identity?.Name?.Split('\\').Last();

                var appUserData = new AppUserData(user, Settings?.Value);

                FullName = appUserData.UserFullName;

            }

          
        }


       
    }
}
