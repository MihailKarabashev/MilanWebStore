namespace MilanWebStore.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class UserOrdersViewModel
    {
        public IEnumerable<MyOrderViewModel> Orders { get; set; }
    }
}
