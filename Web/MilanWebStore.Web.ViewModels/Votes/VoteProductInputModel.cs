namespace MilanWebStore.Web.ViewModels.Votes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static MilanWebStore.Common.ModelValidation.Vote;

    public class VoteProductInputModel
    {
        public int ProductId { get; set; }

        [Required]
        [Range(VoteMinValue, VoteMaxValue)]
        public byte Value { get; set; }
    }
}
