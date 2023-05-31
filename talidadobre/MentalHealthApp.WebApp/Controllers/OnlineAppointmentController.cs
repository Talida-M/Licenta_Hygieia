using AutoMapper;
using MentalHealthApp.BusinessLogic.Implementation;
using MentalHealthApp.BusinessLogic.Implementation.PayPal;
using MentalHealthApp.WebApp.Code.Base;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;
using System.Configuration;
using System;
using System.Collections.Generic;
using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.PayPal.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels;

namespace MentalHealthApp.WebApp.Controllers
{
    public class OnlineAppointmentController : BaseController
    {
        private readonly ForumService _forum;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly PaymentService _paymentService;
        public OnlineAppointmentController(ControllerDependencies dependencies,
                               ForumService forum,
                               IMapper mapper,
                               IConfiguration configuration,
                               PaymentService paymentService )
            : base(dependencies)
        {
            _mapper = mapper;
            _forum = forum;
            _configuration = configuration;
            _paymentService = paymentService;

        }

        [HttpGet]
        public IActionResult VideoDiscussion(int id)
        {
            var role = _forum.GetRole((Guid)CurrentUser.Id);
            ViewData["IsInitiator"] = role == "Specialist";
            ViewData["id"] = id;
            return View("VideoDiscussion");
        }


        public async Task<IActionResult> InitiatePayment(int id)//int pret
        {
            // Create a new APIContext instance using your PayPal credentials
            var apiContext = new APIContext(new OAuthTokenCredential(
                _configuration["PayPalSettings:ClientId"],
                _configuration["PayPalSettings:ClientSecret"]
            ).GetAccessToken());
            var appointmentResult = _paymentService.GetAppointmentDetail(id);
            // Create a new payment object
            decimal conversion = await  _paymentService.ConvertCurrencyAsync(appointmentResult.Pret, "RON", "EUR");
            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction>
        {
            new Transaction
            {
                amount = new Amount
                {
                    total = appointmentResult.Pret.ToString(), //pret.ToString(),
                    currency = "RON"
                },
                description = "Plata realizata de catre " + appointmentResult.Pacient + " pentru consultatia din " + appointmentResult.AppointmentDate + " la medicul " + appointmentResult.Doctor ,
            }
        },
                redirect_urls = new RedirectUrls
                {
                    return_url = Url.Action("VideoDiscussion", id),
                    cancel_url = "https://example.com/cancel"
                }
            };

            // Create the payment
            var createdPayment = payment.Create(apiContext);

            // Redirect the user to the PayPal payment approval URL
            return Redirect(createdPayment.links.FirstOrDefault(l => l.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase)).href);
        }


        public IActionResult ExecutePayment(string paymentId, string token, string PayerID)
        {
            // Create a new APIContext instance using your PayPal credentials
            var apiContext = new APIContext(new OAuthTokenCredential(
                _configuration["PayPalSettings:ClientId"],
            _configuration["PayPalSettings:ClientSecret"]
            ).GetAccessToken());

            // Retrieve the payment by paymentId
            var payment = Payment.Get(apiContext, paymentId);

            // Execute the payment
            var executedPayment = payment.Execute(apiContext, new PaymentExecution { payer_id = PayerID });
            var id = _paymentService.GetAppointmentByPacient().Id;
            // Handle the payment execution response
            if (executedPayment.state == "approved")
            {
                // Payment is successful
                // Perform necessary actions (e.g., update database, send confirmation email, etc.)
    
                var paymentModel = new PaymentModel
                {
                    Amount = int.Parse(payment.transactions[0].amount.total),
                    AppointmentId = id, // Replace with your actual appointment ID
                    Currency = payment.transactions[0].amount.currency,
                    PaymentStatus = executedPayment.state
                };
                _paymentService.CreateTransactionDetail(paymentModel);
                ViewBag.TransactionIsMade = true;
                return RedirectToAction("UserTodayAppointments",  "ProfileController", true);

                    //RedirectToAction("PaymentSuccess");
            }
            else
            {
                // Payment failed
                return RedirectToAction("PaymentFailure");
            }
        }
    }
}
