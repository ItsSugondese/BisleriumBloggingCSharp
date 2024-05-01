using Application.Blogging.TemporaryAttachement;
using Domain.Blogging.Constant;
using Domain.Blogging.enums;
using Domain.Blogging.view.temporary_attachments;
using Microsoft.AspNetCore.Mvc;
using Presentation.Blogging.Generics;

namespace Presentation.Blogging.Controllers
{
    [Route("api/temporary-attachments")]
    [ApiController]
    public class TemporaryAttachmentsController : GenericController
    {
        private readonly ITemporaryAttachmentsService temporaryAttachmentsService;
        private string moduleName;
        public TemporaryAttachmentsController(ITemporaryAttachmentsService temporaryAttachmentsService)
        {
            this.temporaryAttachmentsService = temporaryAttachmentsService;
            this.moduleName = ModuleNameConstant.TEMPORARY_ATTACHMENTS;
        }


      

        // POST api/<TemporaryAttachmentsController>
        [HttpPost]
        public async Task<Object> SaveTemporaryAttachments([FromForm] TemporaryAttachmentsViewModel requestPojo)
        {
            List<int> ids = await temporaryAttachmentsService.SaveTemporaryAttachment(requestPojo);
            return SuccessResponse(MessageConstantMerge.requetMessage(MessageConstant.GET, moduleName),
                CrudStatus.SAVE,
               ids);
        }

        // PUT api/<TemporaryAttachmentsController>/5
      
    }
}
