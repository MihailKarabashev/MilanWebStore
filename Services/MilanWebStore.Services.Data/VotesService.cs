namespace MilanWebStore.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;

        public VotesService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public double GetAverageVote(int productId)
        {
            return this.votesRepository.All().
                Where(x => x.ProductId == productId)
                .Average(x => x.Value);
        }

        public async Task VoteAsync(int productId, string userId, byte value)
        {
            var vote = this.votesRepository.All().
                FirstOrDefault(x => x.ProductId == productId && x.ApplicationUserId == userId);

            if (vote == null)
            {
                vote = new Vote()
                {
                    ProductId = productId,
                    ApplicationUserId = userId,
                };

                await this.votesRepository.AddAsync(vote);
            }

            vote.Value = value;

            await this.votesRepository.SaveChangesAsync();
        }
    }
}
