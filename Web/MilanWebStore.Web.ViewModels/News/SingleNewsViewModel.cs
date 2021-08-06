namespace MilanWebStore.Web.ViewModels.News
{
    using System;

    using MilanWebStore.Services.Mapping;

    public class SingleNewsViewModel : IMapFrom<MilanWebStore.Data.Models.News>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                return this.Description.Length > 90
                     ? this.Description.Substring(0, 90) + "..."
                     : this.Description;
            }
        }

        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ShortTitle { get; set; }
    }
}
