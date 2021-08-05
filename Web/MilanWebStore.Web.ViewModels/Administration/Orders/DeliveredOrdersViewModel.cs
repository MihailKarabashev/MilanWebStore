namespace MilanWebStore.Web.ViewModels.Administration.Orders
{
    using System.Collections.Generic;

    using MilanWebStore.Web.ViewModels.Orders;

    public class DeliveredOrdersViewModel
    {
        public IEnumerable<MyOrderViewModel> Orders { get; set; }
    }
}
