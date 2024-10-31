using FoodOrder.Shared.FoodOrderModel;

namespace FoodOrder.Client.Services
{
    public interface iFoodOrderServices
    {
        ResultLogin activeUsers { get; set; }
        Task<ResultModel<ResultLogin>> LoginUsers(RequestLogin login);
        Task<ResultModel<List<MasterItem>>> GetMasterItem(QueryModel<GetMenu> data, string Token);
        Task<ResultModel<List<getCategory>>> GetCategory(QueryModel<GetMenu> data, string Token);
        Task<ResultModel<ReturnMessage>> UpdateTersedia(string token, QueryModel<GetMenu> File);

        Task<ResultModel<ReturnMessage>> SentCreateMenu(string token, QueryModel<AddNewMenu> File);
        Task<ResultModel<ReturnMessage>> SentOrderNew(string token, QueryModel<SentOrder> File);

        Task<ResultModel<List<GetOrderData>>> GetHeaderOrder(string Token, QueryModel<AGetOrderModel> data);
        Task<ResultModel<int?>> GetDataOrderTotalRow(string token, QueryModel<AGetOrderModel> data);

        Task<ResultModel<List<getOrderDetail>>> GetDetailOrder(QueryModel<GetDetailOrder> data, string Token);

        Task<ResultModel<ReturnMessage>> SentAddOrderNew(string token, QueryModel<SentOrder> File);


        Task<ResultModel<ReturnMessage>> UpdateOrder(string token, QueryModel<updateOrderDetail> File);

        Task<ResultModel<ReturnMessage>> UpdatePembayaran(string token, QueryModel<updatePembayaran> File);


        Task<ResultModel<ReturnMessage>> DeleteItem(string token, QueryModel<DeleteItem> File);
    }
}
