
using eAdmin.Pages;
using System.Threading.Tasks;

namespace eAdmin.Pages.UserAndPermissions
{
    public class PageRoles : PageCore
    {
        protected override async Task OnInitializedAsync()
        {
            is_loading = true;
            await Task.Delay(1000);
            is_loading = false;
        }
    }
}
