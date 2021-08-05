namespace MilanWebStore.Services.Data.Contracts
{
    using MilanWebStore.Data.Models;

    public interface IUsersService
    {
        ApplicationUser GetUser(string username);

        ApplicationUser GetUserById(string id);
    }
}
