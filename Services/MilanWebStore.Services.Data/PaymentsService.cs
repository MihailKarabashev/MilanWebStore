namespace MilanWebStore.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Payments;
    using Stripe;

    public class PaymentsService : IPaymentsService
    {
        private readonly IConfiguration configuration;

        public PaymentsService(
            IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> ChargeAsync(PayInputModel payModel)
        {
            StripeConfiguration.ApiKey = this.configuration.GetSection("Stripe")["SecretKey"];

            var options = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = payModel.CardNumder,
                    ExpMonth = payModel.Month,
                    ExpYear = payModel.Year,
                    Cvc = payModel.CVC,
                },
            };

            var serviceToken = new TokenService();
            Token stripeToken = await serviceToken.CreateAsync(options);

            var chargeOptions = new ChargeCreateOptions
            {
                Amount = (long)payModel.Amount * 100,
                Currency = "usd",
                Description = "My Description",
                Source = stripeToken.Id,
            };

            var chargeService = new ChargeService();
            Charge charge = await chargeService.CreateAsync(chargeOptions);

            if (charge.Paid)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }
    }
}
