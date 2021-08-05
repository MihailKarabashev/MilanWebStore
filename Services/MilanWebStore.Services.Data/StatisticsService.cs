namespace MilanWebStore.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.ViewModels.Administration.Statistics;

    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository<OrderProduct> orderProductsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly MilanWebStore.Data.Common.IDbQueryRunner queryRunner;

        public StatisticsService(
            IRepository<OrderProduct> orderProductsRepository,
            MilanWebStore.Data.Common.IDbQueryRunner queryRunner,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.orderProductsRepository = orderProductsRepository;
            this.queryRunner = queryRunner;
            this.usersRepository = usersRepository;
        }


        public IEnumerable<object> GetTop5MostCommentedProducts()
        {
            return this.queryRunner.
               RawSqlQuery(
               @" SELECT TOP(5) p.Name, 
                COUNT(c.ProductId) AS CommentCount
                FROM Comments as c
	           INNER JOIN  Products as p ON c.ProductId = p.Id
               GROUP BY p.Name
               ORDER BY  CommentCount DESC",
               x => new { productName = (string)x[0], commentsCount = (int)x[1] });
        }

        public IEnumerable<object> GetBestSellingProducts()
        {
            return this.orderProductsRepository.All()
                .GroupBy(x => new { x.Product.Name })
                .Select(x => new
                {
                    Name = x.Key.Name,
                    Quantity = x.Sum(a => a.Quantity),
                }).OrderByDescending(x => x.Quantity).Take(5).ToList();
        }

        public IEnumerable<object> GetTop5МostProductsByChildCategories()
        {
            return this.queryRunner.RawSqlQuery(
#pragma warning disable SA1117 // Parameters should be on same line or separate lines
                @"SELECT TOP (5) c.Name, COUNT(p.ChildCategoryId)
                   FROM Products as p 
                  INNER JOIN ChildCategories as c ON c.Id = p.ChildCategoryId
                 GROUP BY c.Name
                 ORDER BY COUNT(p.ChildCategoryId) DESC", x => new { categoryName = (string)x[0], productsCount = (int)x[1] });
#pragma warning restore SA1117 // Parameters should be on same line or separate lines
        }

        public IEnumerable<StatisticViewModel> GetTop5LastRegisteredUsers()
        {
            return this.usersRepository.All()
                .Select(x => new StatisticViewModel()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Date = x.CreatedOn,
                }).Take(5).OrderByDescending(x => x.Date).ToList();
        }
    }
}
