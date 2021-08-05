namespace MilanWebStore.Web.ViewModels.Sizes
{
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class SizeViewModel : IMapFrom<Size>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
