namespace MilanWebStore.Web.ViewModels.Administration.Sizes
{
    using System.Collections.Generic;

    using MilanWebStore.Web.ViewModels.Sizes;

    public class AllSizesViewModel
    {
        public IEnumerable<SizeViewModel> Sizes { get; set; }
    }
}
