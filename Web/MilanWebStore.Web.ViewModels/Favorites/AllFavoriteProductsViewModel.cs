namespace MilanWebStore.Web.ViewModels.Favorites
{
    using System.Collections.Generic;

    public class AllFavoriteProductsViewModel
    {
        public IEnumerable<FavoriteProductViewModel> Favorites { get; set; }
    }
}
