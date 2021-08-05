namespace MilanWebStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using MilanWebStore.Web.ViewModels.Payments;

    public interface IPaymentsService
    {
        Task<string> ChargeAsync(PayInputModel payModel);
    }
}
