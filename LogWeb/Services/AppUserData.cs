using System.DirectoryServices.AccountManagement;
using LogWeb.Models;

namespace LogWeb.Services
{
    public class AppUserData
    {
       
        public string? UserFullName { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public AppUserData(string? userName, AppSettings? settings)
        {
            if (settings is { IsLocal: true })
            {
                using var pcLocal = new PrincipalContext(ContextType.Machine);

                var up = UserPrincipal.FindByIdentity(pcLocal, userName ?? "unknown");

                var dirEntry = (System.DirectoryServices.DirectoryEntry)up?.GetUnderlyingObject()!;
                UserFullName = up?.DisplayName;

            }
            else
            {
                using var pc = new PrincipalContext(ContextType.Domain);
                var up = UserPrincipal.FindByIdentity(pc, userName ?? "unknown");

                var dirEntry = (System.DirectoryServices.DirectoryEntry)up?.GetUnderlyingObject()!;
                UserFullName = up?.DisplayName;
            }
           
        }
    }
}
