namespace MilanWebStore.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task VoteAsync(int productId, string userId, byte value);

        double GetAverageVote(int productId);
    }
}
