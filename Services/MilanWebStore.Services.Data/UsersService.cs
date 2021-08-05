namespace MilanWebStore.Services.Data
{
    using System.Linq;

    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public ApplicationUser GetUser(string username)
        {
            return this.usersRepository.All().FirstOrDefault(x => x.Email == username);
        }

        public ApplicationUser GetUserById(string id)
        {
            return this.usersRepository.All().FirstOrDefault(x => x.Id == id);
        }
    }
}
