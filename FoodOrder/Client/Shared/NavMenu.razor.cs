using Microsoft.AspNetCore.Components;

namespace FoodOrder.Client.Shared
{
    public partial class NavMenu : ComponentBase
    {
        protected override async Task OnInitializedAsync()
        {
            StateHasChanged();
        }

        private async void confirmLogout()
        {
            sessionStorage.Clear();
            irestService.activeUsers.Id = 0;
            irestService.activeUsers.Name = "";
            irestService.activeUsers.Username = "";
            irestService.activeUsers.Role = "";
            irestService.activeUsers.Token = "";


            navigate.NavigateTo("/");
        }
    }
}
