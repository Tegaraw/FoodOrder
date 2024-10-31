using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FoodOrder.Shared.FoodOrderModel;
using System.Net.Http.Json;

namespace FoodOrder.Client.Services
{
    public class FoodOrderServices : iFoodOrderServices
    {
        private readonly HttpClient _http;

        public FoodOrderServices(HttpClient http)
        {
            _http = http;
        }

        public ResultLogin activeUsers { get; set; }

        public async Task<ResultModel<ResultLogin>> LoginUsers(RequestLogin data)
        {
            ResultModel<ResultLogin> res = new ResultModel<ResultLogin>();
            try
            {
                _http.DefaultRequestHeaders.Clear();
                //_http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                string jsong = JsonConvert.SerializeObject(data);
                var resault = await _http.PostAsJsonAsync<RequestLogin>("api/ToRestapi/LoginUsers", data);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ResultLogin>>();

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }


        public async Task<ResultModel<List<MasterItem>>> GetMasterItem(QueryModel<GetMenu> data, string Token)
        {
            ResultModel<List<MasterItem>> res = new ResultModel<List<MasterItem>>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
                var result = await _http.PostAsJsonAsync<QueryModel<GetMenu>>($"api/ToRestapi/GetMasterItem", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<MasterItem>>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                }
                else
                {
                    res.Data = null;
                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

            }
            return res;
        }


        public async Task<ResultModel<List<getCategory>>> GetCategory(QueryModel<GetMenu> data, string Token)
        {
            ResultModel<List<getCategory>> res = new ResultModel<List<getCategory>>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
                var result = await _http.PostAsJsonAsync<QueryModel<GetMenu>>($"api/ToRestapi/GetCategory", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<getCategory>>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                }
                else
                {
                    res.Data = null;
                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

            }
            return res;
        }
        public async Task<ResultModel<ReturnMessage>> UpdateTersedia(string token, QueryModel<GetMenu> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<GetMenu>>($"api/ToRestapi/UpdateTersedia", File);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();

                res.Data = result.Data;
                res.isSuccess = result.isSuccess;
                res.ErrorCode = result.ErrorCode;
                res.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }

        public async Task<ResultModel<ReturnMessage>> SentCreateMenu(string token, QueryModel<AddNewMenu> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<AddNewMenu>>($"api/ToRestapi/SentCreateMenu", File);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();

                res.Data = result.Data;
                res.isSuccess = result.isSuccess;
                res.ErrorCode = result.ErrorCode;
                res.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }


        public async Task<ResultModel<ReturnMessage>> SentOrderNew(string token, QueryModel<SentOrder> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<SentOrder>>($"api/ToRestapi/SentOrderNew", File);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();

                res.Data = result.Data;
                res.isSuccess = result.isSuccess;
                res.ErrorCode = result.ErrorCode;
                res.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }

        public async Task<ResultModel<List<GetOrderData>>> GetHeaderOrder(string Token, QueryModel<AGetOrderModel> data)
        {
            ResultModel<List<GetOrderData>> res = new ResultModel<List<GetOrderData>>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
                var result = await _http.PostAsJsonAsync<QueryModel<AGetOrderModel>>($"api/ToRestapi/GetHeaderOrder", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<GetOrderData>>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                }
                else
                {
                    res.Data = null;
                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

            }
            return res;
        }
        public async Task<ResultModel<int?>> GetDataOrderTotalRow(string token, QueryModel<AGetOrderModel> data)
        {
            ResultModel<int?> res = new ResultModel<int?>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await _http.PostAsJsonAsync<QueryModel<AGetOrderModel>>($"api/ToRestapi/GetDataOrderTotalRow", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<int?>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
        public async Task<ResultModel<List<getOrderDetail>>> GetDetailOrder(QueryModel<GetDetailOrder> data, string Token)
        {
            ResultModel<List<getOrderDetail>> res = new ResultModel<List<getOrderDetail>>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
                var result = await _http.PostAsJsonAsync<QueryModel<GetDetailOrder>>($"api/ToRestapi/GetDetailOrder", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<getOrderDetail>>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                }
                else
                {
                    res.Data = null;
                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

            }
            return res;
        }


        public async Task<ResultModel<ReturnMessage>> SentAddOrderNew(string token, QueryModel<SentOrder> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<SentOrder>>($"api/ToRestapi/SentAddOrderNew", File);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();

                res.Data = result.Data;
                res.isSuccess = result.isSuccess;
                res.ErrorCode = result.ErrorCode;
                res.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }

        public async Task<ResultModel<ReturnMessage>> UpdateOrder(string token, QueryModel<updateOrderDetail> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<updateOrderDetail>>($"api/ToRestapi/UpdateOrder", File);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();

                res.Data = result.Data;
                res.isSuccess = result.isSuccess;
                res.ErrorCode = result.ErrorCode;
                res.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }



        public async Task<ResultModel<ReturnMessage>> UpdatePembayaran(string token, QueryModel<updatePembayaran> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<updatePembayaran>>($"api/ToRestapi/UpdatePembayaran", File);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();

                res.Data = result.Data;
                res.isSuccess = result.isSuccess;
                res.ErrorCode = result.ErrorCode;
                res.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }


        public async Task<ResultModel<ReturnMessage>> DeleteItem(string token, QueryModel<DeleteItem> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<DeleteItem>>($"api/ToRestapi/DeleteItem", File);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();

                res.Data = result.Data;
                res.isSuccess = result.isSuccess;
                res.ErrorCode = result.ErrorCode;
                res.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
    }
}
