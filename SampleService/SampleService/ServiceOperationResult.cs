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
        public bool CheckExeption { get; set; }
        public string Result { get; set; }

        public ServiceOperationResult(string result)
        {
            Result = result;
        }

        
    }
}