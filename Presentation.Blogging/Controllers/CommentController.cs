﻿using Application.Blogging.BlogApp;
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
        private readonly ICommentReactMappingService _commentReactService;
        private string moduleName;
        public CommentController(ICommentService commentService, ICommentReactMappingService commentReactService)
        {
            this._commentService = commentService;
            this.moduleName = ModuleNameConstant.COMMENT;
            _commentReactService = commentReactService;
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
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName + " Reaction"),
                CrudStatus.SAVE,
               true);
        }
    }
}
