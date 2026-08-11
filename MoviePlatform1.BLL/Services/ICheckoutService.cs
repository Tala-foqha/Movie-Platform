using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> processCheckout(string userId, CheckoutRequest request);
        Task<CheckoutResponse> HandleSuccess(string sessionId);
    }
}
