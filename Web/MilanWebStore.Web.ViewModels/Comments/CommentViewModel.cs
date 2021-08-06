namespace MilanWebStore.Web.ViewModels.Comments
{
    using System.Linq;

    using AutoMapper;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Mapping;

    public class CommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public int ProductId { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }

        public string UserImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, CommentViewModel>()
                .ForMember(x => x.CreatedOn, y => y.MapFrom(c => c.CreatedOn.ToString("g")))
                .ForMember(x => x.FullName, y => y.MapFrom(f => f.ApplicationUser.FirstName + " " + f.ApplicationUser.LastName))
                .ForMember(x => x.UserImageUrl, y => y.MapFrom(x => x.ApplicationUser.Images.FirstOrDefault().RemoteUrl != null
               ? x.ApplicationUser.Images.FirstOrDefault().RemoteUrl
               : "/images/users/" + x.ApplicationUser.Images.FirstOrDefault().Id + "." + x.ApplicationUser.Images.FirstOrDefault().Extention));
        }
    }
}
