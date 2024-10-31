using Microsoft.AspNetCore.Components;

namespace FoodOrder.Client.Pages
{
    public partial class Home : ComponentBase
    {
        public string Name = "";
        protected override async Task OnInitializedAsync()
        {
            Name = "";
            Name = irestService.activeUsers.Name;
            StateHasChanged();
        }
    }
}
