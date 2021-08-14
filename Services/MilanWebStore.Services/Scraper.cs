namespace MilanWebStore.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AngleSharp;
    using AngleSharp.Dom;
    using MilanWebStore.Data.Models;

    public class Scraper
    {
        public async Task<List<News>> Scrape(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var document = await context.OpenAsync(url);

            var main = document.QuerySelector("#tdi_43");

            var elements = main.QuerySelectorAll("div.td-block-span12");
            var allNews = new List<News>();

            foreach (var element in elements)
            {
                var link = element.QuerySelector("div.td_module_6 > div.item-details > h3.entry-title > a").GetAttribute("href");

                var originalPage = new BrowsingContext(config).
                  OpenAsync(link)
                  .GetAwaiter()
                  .GetResult();

                var news = this.GetDescription(originalPage);

                if (news != null && news.Description.Length != 0)
                {
                    allNews.Add(news);
                }
            }

            return allNews;
        }

        private News GetDescription(IDocument originalPage)
        {
            if (originalPage == null)
            {
                return null;
            }

            var news = new News();

            var divMainElement = originalPage.QuerySelector("div.td-ss-main-content");

            (var description, var shortTitle) = this.ExtractDesctiptionAndShortTitle(divMainElement);

            var title = this.GetTitle(divMainElement);

            var image = this.GetImageUrl(divMainElement);

            if (title == null || description == null || image == null)
            {
                return null;
            }

            news.Title = title;
            news.Description = description;
            news.ImageUrl = image;
            news.ShortTitle = shortTitle;

            return news;
        }

        private (string, string) ExtractDesctiptionAndShortTitle(IElement divElement)
        {
            var sb = new StringBuilder();
            var counter = 0;
            string shortDescription = null;

            try
            {
                var pElements = divElement.QuerySelectorAll("div.td-post-content > p");

                foreach (var element in pElements)
                {
                    if (counter == 0)
                    {
                        shortDescription = element.TextContent;
                        counter++;
                        continue;
                    }

                    sb.AppendLine(element.TextContent);
                }

                return (sb.ToString().TrimEnd(), shortDescription);
            }
            catch (System.Exception)
            {

                return (null, null);
            }
        }

        private string GetTitle(IElement divElement)
        {
            if (divElement == null)
            {
                return null;
            }

            var title = divElement.QuerySelector("header.td-post-title > h1.entry-title").TextContent;

            return title;
        }

        private string GetImageUrl(IElement divElement)
        {
            if (divElement == null)
            {
                return null;
            }

            var image = divElement.QuerySelector("figure > a > img").GetAttribute("src");

            return image;
        }
    }
}
