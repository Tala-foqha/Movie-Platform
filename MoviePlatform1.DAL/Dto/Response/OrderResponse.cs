using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public PaymentMethodEnum Payment { get; set; }

        public DateTime OrderDate { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; } = new();
    }
}
