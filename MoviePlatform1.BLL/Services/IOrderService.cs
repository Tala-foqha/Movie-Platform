using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetUserOrders(string userId);//for user
        Task<OrderDetailsResponse?> GetUserOrder(string userId, int orderId);
        Task<bool> CancelOrder(string userId, int orderId);
        Task<List<OrderResponse>> GetAllOrders(OrderStatus status);
        Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatus request);
    }
}
