using CommonLib.Models;
using CommonLib.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SampleService
{
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class Saver : ISaver 
    {
        ServiceOperationResult operationResult = new ServiceOperationResult(); 
  
        public ServiceOperationResult DBSave(Immovables im)
        {
            try
            {
                ImmoRepos ir = new ImmoRepos();
                ir.Update(im.Id, im);
            }
            catch (Exception e)
            {
                operationResult.Message = e.Message;
                operationResult.IsSuccess = true; 
            }
            return operationResult;
        }
    }
}
