namespace MilanWebStore.Web.ViewModels.Administration.Orders
{
    using System.Collections.Generic;

    using MilanWebStore.Web.ViewModels.Orders;

    public class IndexOrdersViewModel
    {
        public IEnumerable<MyOrderViewModel> ProcessedOrders { get; set; }

        public IEnumerable<MyOrderViewModel> UnProcessedOrders { get; set; }
    }
}
