using Application.Blogging.BlogApp;
using Application.Blogging.RepoInterface.BloggingRepoInterface;
using Application.Blogging.TemporaryAttachement;
using Domain.Blogging.Constant;
using Domain.Blogging.Entities;
using Domain.Blogging.enums;
using Domain.Blogging.view.BLogView;
using Domain.Blogging.view.BLogView.PaginationViewForBLog;
using Domain.Blogging.view.temporary_attachments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Blogging.Generics;

namespace Presentation.Blogging.Controllers
{
    
    [Route("api/blog")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BlogController : GenericController
    {
        private readonly IBlogService _blogService;
        private readonly IBlogRepo _blogRepo;
        private readonly IBlogReactMappingService _blogReactService;
        public BlogController(IBlogService blogService, IBlogReactMappingService blogReactService, IBlogRepo blogRepo)
        {
            this.moduleName = ModuleNameConstant.BLOG;
            _blogReactService = blogReactService;
            _blogService = blogService;
            _blogRepo = blogRepo;
        }




        [HttpPost]
        public async Task<Object> SaveBlog(BlogViewModel requestPojo)
        {
             await _blogService.saveBlog(requestPojo);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName),
                CrudStatus.SAVE,
               true);
        }
        
        [HttpPost("paginated")]
        public async Task<Object> SaveBlog(BlogPaginationViewModel requestPojo)
        {
            
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName),
                CrudStatus.GET,
                await _blogService.GetBlogPaginataed(requestPojo));
        }
        
        [HttpDelete("{id}")]
        public async Task<Object> DeleteBlog(int id)
        {
             await _blogService.deleteBlog(id);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName),
                CrudStatus.DELETE,
               true);
        }
        
        [HttpGet("{id}")]
        public async Task<Object> GetBlogById(int id)
        {
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName),
                CrudStatus.GET,
               await _blogService.GetBlogDetailsById(id));
        }
        
        [HttpPost("react")]
        public async Task<Object> ReactOnBlog(BlogReactMappingViewModel requestPojo)
        {
             await _blogReactService.SaveBlogReaction(requestPojo);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName),
                CrudStatus.SAVE,
               true);
        }

        [HttpGet("doc/{id}")]
        public async Task<Object> GetDocs(int id)
        {
            Blog blog = await _blogRepo.FindById(id);
            string? photoPath = blog.ImagePath;

            if (photoPath != null && !string.IsNullOrEmpty(photoPath))
            {
                Byte[] b = System.IO.File.ReadAllBytes(photoPath);
                return File(b, "image/jpeg");
            }

            return null;
        }
    }
}
