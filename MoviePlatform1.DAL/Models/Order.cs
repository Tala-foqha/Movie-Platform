using MoviePlatform1.DAL.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    public enum OrderStatus
    {
        Pending=1,
        Completed=2,
        Cancelled=3
    }
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderStatus OrderStatus { get; set; }

        public PaymentMethodEnum PaymentMethod { get; set; }

        public string? StripeSessionId { get; set; }

        public decimal AmountPaid { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
