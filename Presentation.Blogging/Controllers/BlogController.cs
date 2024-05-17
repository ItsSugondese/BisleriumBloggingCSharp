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
    [ApiController]
    public class BlogController : GenericController
    {
        private readonly IBlogService _blogService;
        private readonly IBlogRepo _blogRepo;
        private readonly IBlogReactMappingService _blogReactService;
        private readonly IBlogHistoryRepo _blogHistoryRepo;
        public BlogController(IBlogService blogService, IBlogReactMappingService blogReactService, IBlogRepo blogRepo,
            IBlogHistoryRepo blogHistoryRepo)
        {
            this.moduleName = ModuleNameConstant.BLOG;
            _blogReactService = blogReactService;
            _blogService = blogService;
            _blogRepo = blogRepo;
            _blogHistoryRepo = blogHistoryRepo;
        }




        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<Object> SaveBlog(BlogViewModel requestPojo)
        {
             await _blogService.saveBlog(requestPojo);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.POST, moduleName),
                CrudStatus.SAVE,
               true);
        }
        
        [HttpPost("paginated")]
        public async Task<Object> GetBlogPaginated(BlogPaginationViewModel requestPojo)
        {
            
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName),
                CrudStatus.GET,
                await _blogService.GetBlogPaginataed(requestPojo));
        }
        
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
            BlogHistory blogHistory = await _blogHistoryRepo.FindById(id);
            string? photoPath = blogHistory.ImagePath;

            if (photoPath != null && !string.IsNullOrEmpty(photoPath))
            {
                Byte[] b = System.IO.File.ReadAllBytes(photoPath);
                return File(b, "image/jpeg");
            }

            return null;
        }
        
        [HttpDelete("doc/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<Object> DeleteDocsPicture(int id)
        {
            await _blogHistoryRepo.deleteImage(id);
            return SuccessResponse("Image removed successfully",
                CrudStatus.DELETE,
               true);
        }
        
        [HttpGet("history/{id}")]
        public  Object GetBlogHistory(int id)
        {
            return SuccessResponse("Image removed successfully",
                CrudStatus.GET,
                _blogHistoryRepo.GetHistoryBasicDetailsByBlogId(id));
        }
    }
}
