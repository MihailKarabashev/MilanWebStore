namespace MilanWebStore.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using MilanWebStore.Data.Common.Models;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Extention { get; set; }

        public string RemoteUrl { get; set; }

        public int? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
