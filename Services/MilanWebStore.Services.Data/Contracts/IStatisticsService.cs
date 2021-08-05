namespace MilanWebStore.Services.Data.Contracts
{
    using System.Collections.Generic;

    using MilanWebStore.Web.ViewModels.Administration.Statistics;

    public interface IStatisticsService
    {
        IEnumerable<object> GetBestSellingProducts();

        IEnumerable<object> GetTop5MostCommentedProducts();

        IEnumerable<object> GetTop5МostProductsByChildCategories();

        IEnumerable<StatisticViewModel> GetTop5LastRegisteredUsers();
    }
}
