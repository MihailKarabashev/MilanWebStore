namespace MilanWebStore.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Services.Mapping;

    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<News> newsRepository;

        public NewsService(IDeletableEntityRepository<News> newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        public T ById<T>(int id)
        {
            return this.newsRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 6)
        {
            return this.newsRepository.All()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>().ToList();
        }

        public int GetNewsCount()
        {
            return this.newsRepository.All().Count();
        }

        public IEnumerable<T> GetTop3LatestNews<T>()
        {
            return this.newsRepository.All().OrderByDescending(x => x.CreatedOn).Take(3).To<T>().ToList();
        }

        public async Task Scrape()
        {
            var scraper = new Scraper();
            var news = await scraper.Scrape("https://www.rossoneriblog.com");

            foreach (var singleNews in news)
            {
                bool isExsist = this.newsRepository.All().Any(x => x.Title == singleNews.Title);

                if (!isExsist)
                {
                    await this.newsRepository.AddAsync(singleNews);
                    await this.newsRepository.SaveChangesAsync();
                }
            }
        }
    }
}
