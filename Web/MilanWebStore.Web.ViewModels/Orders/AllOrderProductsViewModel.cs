namespace MilanWebStore.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class AllOrderProductsViewModel
    {
        public IEnumerable<OrderProductViewModel> Products { get; set; }
    }
}
