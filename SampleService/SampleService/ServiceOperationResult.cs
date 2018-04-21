using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleService
{
    public class ServiceOperationResult
    {
        public ServiceOperationResult()
        {
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}