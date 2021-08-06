namespace MilanWebStore.Web.ViewModels.NavBars
{
    using System.Collections.Generic;

    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class ChildNavViewModel : IMapFrom<ChildCategory>
    {
        public string Name { get; set; }

        public IEnumerable<ParentChildNavViewModel> ParentChildCategory { get; set; }
    }
}
