using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.PayPal.ViewModels;
using MentalHealthApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.PayPal
{
    public class PaymentService : BaseService
    {
        private const string ApiUrl = "https://api.exchangerate-api.com/v4/latest/";

        public PaymentService(ServiceDependencies dependencies) : base(dependencies)
        {}

        public AppointmentDetailVM GetAppointmentDetail(int id)
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.Programari.Get()
                                      .Include(p => p.Specialist)
                                      .Where(p => p.Id == id)
                                     .Select(u => new AppointmentDetailVM
                                     {
                                         Id = u.Id,
                                         AppointmentDate = u.AppointmentDate,
                                         Doctor = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         Pacient = CurrentUser.FirstName,
                                         Pret = uow.Specialisti.Get().Where(n => n.SpecialistId.Equals(u.SpecialistId)).Select(n => n.Price).Single(),

                                     })
                                    .First();


            });
        }



        public async Task<decimal> ConvertCurrencyAsync(decimal amount, string baseCurrency, string targetCurrency)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string requestUrl = $"{ApiUrl}/{baseCurrency}";
                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseJson);

                        decimal rate = data.rates[targetCurrency];
                        decimal convertedAmount = amount * rate;

                        return convertedAmount;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exception that may occur during the API call
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return amount; // Return the original amount if conversion fails
        }
    


    public AppointmentDetailVM GetAppointmentByPacient()
        {
            return ExecuteInTransaction(uow =>
            {
                var currentDate = DateTime.Today.Date;

                return uow.Programari.Get()
                                      .Include(p => p.Specialist)
                                      .Where(p => p.UserId == CurrentUser.Id && (Convert.ToDateTime(p.AppointmentDate).Day == currentDate.Day && Convert.ToDateTime(p.AppointmentDate).Month == currentDate.Month && Convert.ToDateTime(p.AppointmentDate).Year == currentDate.Year))
                                     .Select(u => new AppointmentDetailVM
                                     {
                                         Id = u.Id,
                                         AppointmentDate = u.AppointmentDate,
                                         Doctor = uow.IdentityUsers.Get().Where(n => n.Id.Equals(u.SpecialistId)).Select(n => $"{n.LastName} {n.FirstName}").Single(),
                                         Pacient = CurrentUser.FirstName,
                                         Pret = uow.Specialisti.Get().Where(n => n.SpecialistId.Equals(u.SpecialistId)).Select(n => n.Price).Single(),

                                     })
                                    .First();


            });
        }
        public void CreateTransactionDetail(PaymentModel model)
        {
            ExecuteInTransaction(uow =>
            {
                var tranzactie = new PaymentDetails
                {
                    Id = new Guid(),
                    Amount = model.Amount,
                    AppointmentId = model.AppointmentId,
                    PaymentDate = DateTime.Now,
                    Currency = model.Currency,
                    PaymentStatus = model.PaymentStatus
                };
                uow.PaymentsDetails.Insert(tranzactie);
                uow.SaveChanges();
            });
        }
    }
}
