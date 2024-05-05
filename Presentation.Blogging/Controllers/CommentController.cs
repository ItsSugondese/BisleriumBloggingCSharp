using Application.Blogging.BlogApp;
using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Application.RepoInterface.BlogginRepoInterface;
using Domain.Blogging.Constant;
using Domain.Blogging.enums;
using Domain.Blogging.view.BLogView;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Blogging.Generics;

namespace Presentation.Blogging.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentController : GenericController
    {
        private readonly ICommentService _commentService;
        private readonly ICommentRepo _commentRepo;
        private readonly ICommentHistoryRepo _commentHistoryRepo;
        private readonly ICommentReactMappingService _commentReactService;
        public CommentController(ICommentService commentService, ICommentReactMappingService commentReactService,
            ICommentRepo commentRepo, ICommentHistoryRepo commentHistoryRepo)
        {
            this._commentService = commentService;
            this.moduleName = ModuleNameConstant.COMMENT;
            _commentReactService = commentReactService;
            _commentRepo = commentRepo;
            _commentHistoryRepo = commentHistoryRepo;
        }




        [HttpPost]
        public async Task<Object> SaveComment(CommentViewModel requestPojo)
        {
            await _commentService.saveComment(requestPojo);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName),
                CrudStatus.SAVE,
               true);
        }

        [HttpPost("react")]
        public async Task<Object> ReactOnBlog(CommentReactMappingViewModel requestPojo)
        {
            await _commentReactService.SaveCommentReaction(requestPojo);
            return SuccessResponse(moduleName + " Reaction successful",
                CrudStatus.GET,
               true);
        }

        [HttpDelete("{id}")]
        public async Task<Object> DeleteComment(int id)
        {
            await _commentService.deleteComment(id);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName),
                CrudStatus.DELETE,
               true);
        }
        
        [HttpGet("history/{id}")]
        public  Object GetCommentHistory(int id)
        {
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName),
                CrudStatus.DELETE,
               _commentHistoryRepo.GetCommentHistoryBasicDetalsByCommentId(id));
        }
    }
}
