
using MilanWebStore.Data.Common.Models;

namespace MilanWebStore.Data.Models
{
    public class ParentChildCategory : BaseDeletableModel<int>
    {
        public ParentCategory ParentCategory { get; set; }

        public int ParentCateogryId { get; set; }

        public ChildCategory ChildCategory { get; set; }

        public int ChildCategoryId { get; set; }
    }
}
