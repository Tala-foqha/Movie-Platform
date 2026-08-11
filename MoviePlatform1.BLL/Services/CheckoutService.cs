using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using MoviePlatform1.DAL.Repository;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;

namespace MoviePlatform1.BLL.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICartService _cartServices;
        private readonly IMovieRepository _movieRepository;
        private readonly IEmailSender _emailSender;
        private readonly IUserMovieAccessRepository _userMovieAccessRepository;
        public CheckoutService(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository, ICartService cartService, IMovieRepository movieRepository, IEmailSender emailSender, IUserMovieAccessRepository userMovieAccessRepository  )
        {
            _userManager = userManager;
            _movieRepository = movieRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _cartServices = cartService;

            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _userMovieAccessRepository = userMovieAccessRepository;
        }
        public async Task<CheckoutResponse> HandleSuccess(string sessionId)
        {
            // 1. نجيب الـ Order المرتبط بعملية Stripe
            var order = await _orderRepository.Getone(
                o => o.StripeSessionId == sessionId,
                includes: new[]
                {
            nameof(Order.OrderItems)
                });

            // 2. نتأكد إن الـ Order موجود
            if (order == null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Order not found"
                };
            }

            // 3. نتأكد إنه لسه Pending
            if (order.OrderStatus != OrderStatus.Pending)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Order has already been processed"
                };
            }

            // 4. الدفع نجح → نغير حالة الـ Order
            order.OrderStatus = OrderStatus.Completed;

            await _orderRepository.UpdateAsync(order);
            var user = await _userManager.FindByIdAsync(order.UserId);
            await _emailSender.SendEmail(user.Email, "order confirm", "<h2> your order has been placed successfully </h2>");


            // 5. نعطي المستخدم صلاحية مشاهدة كل فيلم اشتراه
            foreach (var item in order.OrderItems)
            {
                var access = new UserMovieAccess
                {
                    UserId = order.UserId,
                    MovieId = item.MovieId,
                    HasAccess=true,
                    
                };

                await _userMovieAccessRepository.CreateAsync(access);
            }

            // 6. نفرغ الكارت بعد نجاح الدفع
            await _cartServices.ClearCart(order.UserId);

            return new CheckoutResponse
            {
                Success = true,
                OrderId = order.Id
            };
        }






        public async Task<CheckoutResponse> processCheckout(string userId, CheckoutRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "User not found"
                };
            }
            var cartItems=await _cartRepository.GetAllAsync(c=>c.UserId == userId,
                new string[]
                {
               nameof(Cart.Movie),
               $"{nameof(Cart.Movie)}.{nameof(Movie.Translations)}"
                }
            );
            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Cart is empty"
                };
            }
            Order order = new Order
            {
                OrderStatus = OrderStatus.Pending,
                UserId = userId,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = (decimal)cartItems.Sum(x => x.Movie.price),
                OrderItems = cartItems.Select(x => new OrderItem
                {
                    MovieId = x.MovieId,
                    Unitprice = (decimal)x.Movie.price,



                }).ToList()


               
            };
            await _orderRepository.CreateAsync(order);
            foreach (var item in cartItems)
            {
                

                if (item.Movie.price <= 0)
                    return new CheckoutResponse { Success = false, Error = "Invalid product price" };
            }
            foreach (var item in cartItems)
            {
                Console.WriteLine($"Product: {item.MovieId}");
                Console.WriteLine($"Price: {item.Movie.price}");
            
            }
            if (request.PaymentMethod == PaymentMethodEnum.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    Mode = "payment",
                    //اذا تمت عمليه الدفع بنجاح لازم احول اليوزر ع مكان معين واذا فشلت نفس الشغله بس ع مكان تاني
                    SuccessUrl =
    $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Checkout/Success?sessionId={{CHECKOUT_SESSION_ID}}",
                    CancelUrl =
    $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Checkout/cancel",
                    //المنتجات يلي بالسله بدي اعطيهم لسترايب عشان يعرهم بالصفحه
                    LineItems = new List<SessionLineItemOptions>()

                };
                foreach (var item in cartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(item.Movie.price * 100), // Stripe expects amount in cents,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Movie.Translations.FirstOrDefault(x => x.Language == "en")?.Title ?? "Product"
                            }
                            
                        },
                        Quantity = 1
                    });
                }
                var service = new Stripe.Checkout.SessionService();
                var session = service.Create(options);
                order.StripeSessionId = session.Id; //لتحديث الطلب
                await _orderRepository.UpdateAsync(order);
                return new CheckoutResponse
                {
                    Success = true,
                    StripeUrl = session.Url//رابط صفحه الدفع يلي راح يفتح ع سترايب
                };
              
            }

            return new CheckoutResponse
            {
                Success = false,
                Error = "Invalid payment method"
            };
        }

    }
    }

