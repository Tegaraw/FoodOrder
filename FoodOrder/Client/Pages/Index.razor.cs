using FoodOrder.Shared.FoodOrderModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;

namespace FoodOrder.Client.Pages
{
    public partial class Index : ComponentBase
    {
        private RequestLogin log { get; set; } = new RequestLogin();
        public RequestLogin lo { get; set; } = new RequestLogin();
        private ResultLogin user { get; set; } = new ResultLogin();

        private bool isLoading = false;
        public string? alertMessage = null;

        private IJSObjectReference _jsModule;

        protected override async Task OnInitializedAsync()
        {
            StateHasChanged();
        }
        private async Task checkLogin()
        {
            isLoading = true;
            await checkdulu();

        }
        private async Task checkdulu()
        {

            try
            {
                RequestLogin reqLogins = new RequestLogin();
                reqLogins.Username = lo.Username;
                reqLogins.Password = lo.Password;
                var a = await irestService.LoginUsers(reqLogins);
                if (a.isSuccess)
                {
                    if (a.Data.Username != "" && a.Data.Username != "")
                    {
                        irestService.activeUsers = new ResultLogin();
                        irestService.activeUsers.Name = a.Data.Name;
                        irestService.activeUsers.Username = a.Data.Username;
                        irestService.activeUsers.Role = a.Data.Role;
                        irestService.activeUsers.Token = a.Data.Token;


                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken((string)a.Data.Token);
                        var tokenz = jsonToken as JwtSecurityToken;
                        sessionStorage.SetItem("Name", a.Data.Name);
                        sessionStorage.SetItem("Username", a.Data.Username);
                        sessionStorage.SetItem("Token", a.Data.Token);
                        sessionStorage.SetItem("Role", a.Data.Role);
                        navigate.NavigateTo("Home");
                        isLoading = false;
                    }
                    else
                    {
                        alertMessage = "Username or Password is wrong";
                        isLoading = false;
                    }
                }
            }
            catch (Exception ex)
            {
                alertMessage = ex.Message;
                isLoading = false;
                StateHasChanged();
                throw;
            }


            StateHasChanged();
        }
        private async void Clear()
        {
            log.Username = "";
            log.Password = "";
            alertMessage = null;
            StateHasChanged();
        }
    }
}


