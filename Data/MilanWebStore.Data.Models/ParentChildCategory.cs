namespace MilanWebStore.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ParentChildCategory
    {
        public int Id { get; set; }

        [ForeignKey(nameof(ParentCateogryId))]
        public ParentCategory ParentCategory { get; set; }

        public int ParentCateogryId { get; set; }

        [ForeignKey(nameof(ChildCategoryId))]
        public ChildCategory ChildCategory { get; set; }

        public int ChildCategoryId { get; set; }
    }
}
