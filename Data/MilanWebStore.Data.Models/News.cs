namespace MilanWebStore.Data.Models
{
    using MilanWebStore.Data.Common.Models;

    public class News : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string ShortTitle { get; set; }
    }
}
