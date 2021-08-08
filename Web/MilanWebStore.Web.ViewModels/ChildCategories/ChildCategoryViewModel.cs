namespace MilanWebStore.Web.ViewModels.ChildCategories
{
    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;
    using System.Linq;

    public class ChildCategoryViewModel : IMapFrom<ChildCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
