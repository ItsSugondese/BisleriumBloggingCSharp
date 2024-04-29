using Domain.Blogging.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view
{
    public class GlobalApiResponse
    {
        //public enums.ResponseStatus Status { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public CrudStatus Crud { get; set; }

        //public void SetResponse(string message, enums.ResponseStatus status, object data)
        public void SetResponse(string message, int status, object data, CrudStatus crud)
        {
            Message = message;
            Status = status;
            Data = data;
            Crud = crud;
        }
    }


}
