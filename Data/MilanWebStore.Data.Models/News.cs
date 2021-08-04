using MilanWebStore.Data.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace MilanWebStore.Data.Models
{
    public class News : BaseDeletableModel<int>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string ShortTitle { get; set; }
    }
}
