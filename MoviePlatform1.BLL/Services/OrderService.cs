using Mapster;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using MoviePlatform1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<bool> CancelOrder(string userId, int orderId)
        {
            var order = await _orderRepository.Getone(
                filter: o => o.UserId == userId && o.Id == orderId
               );
            if (order is null) return false;
            //لسا م توافق ع هاد الطلب 
            if (order.OrderStatus != OrderStatus.Pending)
            {
                return false;
            }
            order.OrderStatus = OrderStatus.Cancelled;
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> ChangeOrderStatus(
    int orderId,
    ChangeOrderStatus status)
        {
            var order = await _orderRepository.Getone(
                o => o.Id == orderId);

            if (order == null)
                return false;

            // إذا الطلب مكتمل أو ملغي، لا يمكن تغيير حالته
            if (order.OrderStatus == OrderStatus.Completed ||
                order.OrderStatus == OrderStatus.Cancelled)
            {
                return false;
            }

            // Pending → Completed
            // Pending → Cancelled
            if (order.OrderStatus == OrderStatus.Pending &&
                (status.Status == OrderStatus.Completed ||
                 status.Status == OrderStatus.Cancelled))
            {
                order.OrderStatus = status.Status;

                return await _orderRepository.UpdateAsync(order);
            }

            return false;
        }
        public async Task<List<OrderResponse>> GetAllOrders(OrderStatus status)
        {

            var orders = await _orderRepository.GetAllAsync(
                filter: o => o.OrderStatus == status

                );
            return orders.Adapt<List<OrderResponse>>();
        }

        public async Task<OrderDetailsResponse?> GetUserOrder(string userId, int orderId)
        {
            var order = await _orderRepository.Getone(
                o=>o.Id==orderId&&o.UserId==userId,
                new string[]
                {
                 nameof(Order.OrderItems),
            $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Movie)}",
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Movie)}.{nameof(Movie.Translations)}"
                }

                );
            return order.Adapt<OrderDetailsResponse?>();
        }

        public async Task<List<OrderResponse>> GetUserOrders(string userId)
        {
            var orders = await _orderRepository.GetAllAsync(
                o=>o.UserId==userId,
                new string[]
                {
               nameof(Order.OrderItems),
            $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Movie)}",
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Movie)}.{nameof(Movie.Translations)}"
                }
                );
            return orders.Adapt<List<OrderResponse>>();
          
        }
    }
}
