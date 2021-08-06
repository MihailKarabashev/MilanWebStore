namespace MilanWebStore.Web.ViewModels.News
{
    using System.Collections.Generic;

    public class AllNewsViewModel : PagingViewModel
    {
        public IEnumerable<SingleNewsViewModel> AllNews { get; set; }
    }
}
