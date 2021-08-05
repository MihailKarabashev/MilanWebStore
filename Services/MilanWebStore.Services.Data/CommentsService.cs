namespace MilanWebStore.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MilanWebStore.Common;
    using MilanWebStore.Data.Common.Repositories;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Services.Mapping;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commnetsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commnetsRepository)
        {
            this.commnetsRepository = commnetsRepository;
        }

        public async Task CreateAsync(int productId, string userId, string content)
        {
            var comment = new Comment()
            {
                ProductId = productId,
                ApplicationUserId = userId,
                Content = content,
                IsDeleted = false,
            };

            await this.commnetsRepository.AddAsync(comment);
            await this.commnetsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int productId)
        {
            return this.commnetsRepository.All().Where(x => x.ProductId == productId).To<T>().ToList();
        }

        public async Task RemoveAsync(int id)
        {
            var comment = this.commnetsRepository.All().Where(x => x.Id == id).FirstOrDefault();

            if (comment == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.CommentNotFound, id));
            }

            comment.ModifiedOn = DateTime.UtcNow;
            comment.IsDeleted = true;

            this.commnetsRepository.Update(comment);
            await this.commnetsRepository.SaveChangesAsync();
        }
    }
}
