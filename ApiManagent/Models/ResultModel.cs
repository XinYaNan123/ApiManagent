using System;

namespace ApiManagent.Models
{
    public class ResultModel
    {
       public int Code { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
