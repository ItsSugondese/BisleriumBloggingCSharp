using Domain.Blogging.enums;
using Domain.Blogging.view;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Blogging.Generics
{
    public class GenericController : ControllerBase
    {
        public string moduleName { get; set; }
        protected readonly ResponseStatus ApiResponseStatus = ResponseStatus.Success;
        protected GlobalApiResponse SuccessResponse(string message, CrudStatus status,  object data)
        {
            GlobalApiResponse globalApiResponse = new GlobalApiResponse
            {
                Status = (int)ApiResponseStatus,
                Message = message,
                Data = data,
                Crud = status
            };
            return globalApiResponse;
        }
    }
}
