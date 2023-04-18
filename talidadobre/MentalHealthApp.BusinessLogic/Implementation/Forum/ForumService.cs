using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Forum.Validations;
using MentalHealthApp.BusinessLogic.Implementation.Forum.ViewModels;
using MentalHealthApp.Common.DTOs;
using MentalHealthApp.Common.Extensions;
using MentalHealthApp.Entities;
using MentalHealthApp.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation
{
    public class ForumService : BaseService
    {
        private readonly DiscussionValidator _discussionValidator;
        public ForumService(ServiceDependencies dependencies) 
                            : base(dependencies) 
        {
            _discussionValidator = new DiscussionValidator();
        }
        public string GetRole(Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.IdentityRoles.Get()
                                        .Include(r => r.Users)
                                        .ThenInclude(ir => ir.Roles)
                                        .Where(r => r.Users.Select(u => u.Id).Contains(id))
                                        .Select(r => r.Name)
                                        .Single();
            });
        }
        public List<DiscussionVM> GetAllDiscussions()
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.Discutii.Get()
                                    .Select(d => new DiscussionVM
                                    {
                                        Id = d.Id,
                                        UserId = d.UserId,
                                        Username = uow.IdentityUsers.Get()
                                                         .Where(u => u.Id.Equals(d.UserId))
                                                          .Select(u => $"{u.LastName} {u.FirstName}")
                                                          .Single(),
                                        Title = d.Title,
                                        MessageContent = d.MessageContent,
                                        CommentDate = d.CommentDate,
                                        UserImage = uow.IdentityUsers.Get().Where(u => u.Id.Equals(d.UserId)).Select(u => u.UserImage).Single(),
                                    })
                                    .OrderByDescending(od => od.CommentDate)
                                   .ToList();  
            });
        }

        public DiscussionVM GetDiscussionById(Guid id)
        {
            
                return ExecuteInTransaction(uow =>
                {
                    return uow.Discutii.Get()
                           .Where(d => d.Id.Equals(id))
                           .Select(d => new DiscussionVM
                           {
                               Id = d.Id,
                               UserId = d.UserId,
                               Username = uow.IdentityUsers.Get()
                                                         .Where(u => u.Id.Equals(d.UserId))
                                                          .Select(u => $"{u.LastName} {u.FirstName}")
                                                          .Single(),
                               Title = d.Title,
                               MessageContent = d.MessageContent,
                               CommentDate = d.CommentDate,
                               UserImage = uow.IdentityUsers.Get().Where(u => u.Id.Equals(d.UserId)).Select(u => u.UserImage).Single(),
                               
                           })
                          .Single();
                });
           
           
        }

        public void CreateDiscussion(CreateDiscussionVM newDiscussion)
        {
            _discussionValidator.Validate(newDiscussion).ThenThrow(newDiscussion);
             ExecuteInTransaction(uow =>
            {
                var discussion = new Discussion();
                discussion.Id = Guid.NewGuid();
                discussion.UserId = (Guid)CurrentUser.Id;
                discussion.Title = newDiscussion.Title;
                discussion.MessageContent = newDiscussion.MessageContent;
                discussion.CommentDate = DateTime.UtcNow;
                uow.Discutii.Insert(discussion);
                uow.SaveChanges();

            });
        }

        public void CreateDiscussionComment(DiscussionVM newComment)
        {
            ExecuteInTransaction(uow =>
            {
                var comment = new DiscussionComment();
                comment.Id = Guid.NewGuid();
                comment.UserId = (Guid)CurrentUser.Id;
                comment.DiscussionId = newComment.Id;
                comment.MessageContent = newComment.MessageContent;
                comment.CommentDate = DateTime.UtcNow;
                uow.ComentariiDiscutii.Insert(comment);
                uow.SaveChanges();
            });
        }
        public List<CreateDiscussionCommentsVM> ViewDiscussionComments(Guid discussionId)
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.ComentariiDiscutii.Get()
                                             .Include(cd => cd.Discussion)
                                             .Include(cd => cd.User)
                                             .Where(cd => cd.DiscussionId.Equals(discussionId))
                                             .Select(cd => new CreateDiscussionCommentsVM
                                             {
                                                 Id = cd.Id,
                                                 UserId = cd.UserId,
                                                 DiscussionTitle = cd.Discussion.Title,
                                                 Username = uow.IdentityUsers.Get()
                                                         .Where(u => u.Id.Equals(cd.UserId))
                                                          .Select(u => $"{u.LastName} {u.FirstName}")
                                                          .Single(),
                                                 MessageContent = cd.MessageContent,
                                                 CommentDate = cd.CommentDate,
                                                 UserImage = uow.IdentityUsers.Get().Where(u => u.Id.Equals(cd.UserId)).Select(u => u.UserImage).Single(),
                                             })
                                             .OrderBy(u => u.CommentDate)
                                             .ToList();
            });
        }
        public int NumberOfComments(Guid discussionId)
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.ComentariiDiscutii.Get()
                                             .Include(cd => cd.Discussion)
                                             .Include(cd => cd.User)
                                             .Where(cd => cd.DiscussionId.Equals(discussionId))
                                             .Count();
                                  
            });
        }
        public async Task<DiscussionComment> DeteleComment(Guid id)
        {
            
            return ExecuteInTransaction(uow =>
            {
                
                    var comment = uow.ComentariiDiscutii.Get()
                                                    .Where(cd => cd.Id.Equals(id))
                                                    .Single();
                    uow.ComentariiDiscutii.Delete(comment);
                    uow.SaveChanges();
                return comment;
                    
            });
        }
        public async Task<Discussion> DeleteDiscussion (Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                var discussion = uow.Discutii.Get()
                                              .Include(d => d.DiscussionComments)
                                              .Where(uow => uow.Id.Equals(id))
                                              .Single();
                uow.Discutii.Delete(discussion);
                uow.SaveChanges();
                return discussion;
            });
        }
        public string EditComment(Guid id, string message)
        {
            return ExecuteInTransaction(uow =>
            {
                var comment = uow.ComentariiDiscutii.Get()
                                                          .Where(cd => cd.Id.Equals(id))
                                                          .SingleOrDefault();
                comment.MessageContent = message;
                uow.ComentariiDiscutii.Update(comment);
                uow.SaveChanges();
                return message;
            });
        }

        public string EditDiscussionContent(Guid id, string message)
        {
            return ExecuteInTransaction(uow =>
            {
                var discussion = uow.Discutii.Get()
                                             .Where(d => d.Id.Equals(id))
                                             .Single();
                discussion.MessageContent = message;
                uow.Discutii.Update(discussion);
                uow.SaveChanges();
                return message;
            });
        }
    }
}
