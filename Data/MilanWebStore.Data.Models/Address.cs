using MilanWebStore.Data.Common.Models;
using System.Collections.Generic;

namespace MilanWebStore.Data.Models
{
    public class Address : BaseModel<int>
    {
        public Address()
        {
            this.Orders = new HashSet<Order>();
        }

        public string City { get; set; }

        public string Street { get; set; }

        public string Notes { get; set; }

        public string ZipCode { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
