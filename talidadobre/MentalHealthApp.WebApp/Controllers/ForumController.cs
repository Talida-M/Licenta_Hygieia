using MentalHealthApp.BusinessLogic.Implementation;
using MentalHealthApp.BusinessLogic.Implementation.Forum.ViewModels;
using MentalHealthApp.Entities.Enums;
using MentalHealthApp.WebApp.Code.Base;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthApp.WebApp.Controllers
{
    public class ForumController : BaseController
    {
        private readonly ForumService _forumService;
       public ForumController(ControllerDependencies dependencies,
                              ForumService forumService)
                             : base(dependencies)
        { 
            _forumService = forumService;
        }

        [HttpGet]
        public IActionResult ViewDiscussionsPage()
        {
            var model = _forumService.GetAllDiscussions();
            return View("DiscussionPage", model);
        }

     
        [HttpGet]
        public IActionResult ViewForumFirstPage()
        {
            var discussionsList = _forumService.GetAllDiscussions();
            var model = new CreateDiscussionVM()
            {
                DiscussionVMs = discussionsList.Select(app => new DiscussionVM
                {
                    UserId = app.UserId,
                    Username = app.Username,
                    Id = app.Id,
                    Title = app.Title,
                    MessageContent = app.MessageContent,
                    CommentDate = app.CommentDate,
                    UserImage = app.UserImage,

                }).ToList()

            };
            return View("ForumFirstPage", model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDiscussion(CreateDiscussionVM model)
        {
        
            _forumService.CreateDiscussion(model);
           // return View("ForumFirstPage", model);
            return RedirectToAction("ViewForumFirstPage");
            //return RedirectToAction("ViewDiscussion", new { id = model.Id });
        }

/// 
        [HttpGet]
        public IActionResult ViewDiscussion(Guid id)
        {
            var discussionDetails = _forumService.GetDiscussionById(id);
            var comments = _forumService.ViewDiscussionComments(id);
            var model = new DiscussionVM()
            {
                Id = discussionDetails.Id,
                UserId = discussionDetails.UserId,
                Username = discussionDetails.Username,
                Title = discussionDetails.Title,
                MessageContent = discussionDetails.MessageContent,
                CommentDate = discussionDetails.CommentDate,
                UserImage = discussionDetails.UserImage,
                createDiscussionCommentsVMs = comments.Select(app => new CreateDiscussionCommentsVM
                {
                    Id = app.Id,
                    UserId = app.UserId,
                    DiscussionId = id,
                    DiscussionTitle = app.DiscussionTitle,
                    Username = app.Username,
                    MessageContent = app.MessageContent,
                    CommentDate = app.CommentDate,
                    UserImage = app.UserImage
                }).ToList()
            };
            return View("DiscussionPage", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscussionComment(DiscussionVM model)
        {

            _forumService.CreateDiscussionComment(model);
            return RedirectToAction("ViewDiscussion", new {id = model.Id});
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDiscussion(Guid id, Guid user)
        {
            var role =  _forumService.GetRole((Guid)CurrentUser.Id);
            if (user.Equals(CurrentUser.Id) || role.Equals(RoleTypes.Admin.ToString()))
            {
                var model = await _forumService.DeleteDiscussion(id);
                return Ok(model.Id);
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "You can not proceed with the action");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteComment(Guid id, Guid user)
        {
            var role = _forumService.GetRole((Guid)CurrentUser.Id);
            if (user.Equals(CurrentUser.Id) || role.Equals(RoleTypes.Admin.ToString()))
            {
                var model = await _forumService.DeteleComment(id);
                return Ok(model.Id);
            }
            return StatusCode(StatusCodes.Status401Unauthorized);

        }

		[HttpGet]
        public IActionResult NumberOfComments(Guid id)
		{
            var comments =  _forumService.NumberOfComments(id);
            return Ok(comments);
		}
        [HttpPatch]
        public IActionResult EditComment(Guid id, string message, Guid user)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var role = _forumService.GetRole((Guid)CurrentUser.Id);
            if (user.Equals(CurrentUser.Id) || role.Equals(RoleTypes.Admin.ToString()))
            {
                var newMessage = _forumService.EditComment(id, message);
                return Ok(newMessage);
            }
            return Ok();
        }

        [HttpPatch]
        public IActionResult EditDiscussionMainComment(Guid id, string message, Guid user)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var role = _forumService.GetRole((Guid)CurrentUser.Id);
            if (user.Equals(CurrentUser.Id) || role.Equals(RoleTypes.Admin.ToString()))
            {
                var newMessage = _forumService.EditDiscussionContent(id, message);
                return Ok(newMessage);
            }
            return Ok();
        }
    }
}
