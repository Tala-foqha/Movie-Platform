using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Request
{
    public enum PaymentMethodEnum
    {
        Visa = 1
    }
    public class CheckoutRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}
