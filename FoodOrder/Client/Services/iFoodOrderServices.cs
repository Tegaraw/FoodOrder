using FoodOrder.Shared.FoodOrderModel;

namespace FoodOrder.Client.Services
{
    public interface iFoodOrderServices
    {
        ResultLogin activeUsers { get; set; }
        Task<ResultModel<ResultLogin>> LoginUsers(RequestLogin login);
        Task<ResultModel<List<MasterItem>>> GetMasterItem(string idjenis, string Token);
        Task<ResultModel<List<getCategory>>> GetCategory(string idjenis, string Token);
        Task<ResultModel<ReturnMessage>> UpdateTersedia(string token, QueryModel<GetMenu> File);

        Task<ResultModel<ReturnMessage>> SentCreateMenu(string token, QueryModel<AddNewMenu> File);
        Task<ResultModel<ReturnMessage>> SentOrderNew(string token, QueryModel<SentOrder> File);

        Task<ResultModel<List<GetOrderData>>> GetHeaderOrder(string Token, string UserName, string Role, int PageNumber, string Condition);


  
        Task<ResultModel<int?>> GetDataOrderTotalRow(string token, string Username, string Role, string filter);

        Task<ResultModel<List<getOrderDetail>>> GetDetailOrder(QueryModel<GetDetailOrder> data, string Token);

        Task<ResultModel<ReturnMessage>> SentAddOrderNew(string token, QueryModel<SentOrder> File);


        Task<ResultModel<ReturnMessage>> UpdateOrder(string token, QueryModel<updateOrderDetail> File);

        Task<ResultModel<ReturnMessage>> UpdatePembayaran(string token, QueryModel<updatePembayaran> File);


        Task<ResultModel<ReturnMessage>> DeleteItem(string token, QueryModel<DeleteItem> File);


        Task<ResultModel<ReturnMessage>> UpdateItem(string token, QueryModel<SentUpdateItemModel> File);

        Task<ResultModel<ReturnMessage>> DeleteItemMaster(string token, string id);


    

        Task<ResultModel<List<GetReportStockData>>> GetReportStok(string Token, int PageNumber, string Condition);


        Task<ResultModel<int?>> GetReportStokTotalRow(string token, string Condition);


        Task<ResultModel<List<GetReportOrderData>>> GetReporOrderData(string Token, int PageNumber, string Condition);

        Task<ResultModel<int?>> GetReportOrderDataTotalRow(string token, string Condition);
    }
}
