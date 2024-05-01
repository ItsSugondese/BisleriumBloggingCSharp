using Application.Blogging.BlogApp;
using Application.Blogging.TemporaryAttachement;
using Domain.Blogging.Constant;
using Domain.Blogging.enums;
using Domain.Blogging.view.BLogView;
using Domain.Blogging.view.temporary_attachments;
using Microsoft.AspNetCore.Mvc;
using Presentation.Blogging.Generics;

namespace Presentation.Blogging.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : GenericController
    {
        private readonly IBlogService _blogService;
        private string moduleName;
        public BlogController(IBlogService temporaryAttachmentsService)
        {
            this._blogService = temporaryAttachmentsService;
            this.moduleName = ModuleNameConstant.BLOG;
        }




        // POST api/<TemporaryAttachmentsController>
        [HttpPost]
        public async Task<Object> SaveBlog(BlogViewModel requestPojo)
        {
             await _blogService.saveBlog(requestPojo);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName),
                CrudStatus.SAVE,
               true);
        }
    }
}
