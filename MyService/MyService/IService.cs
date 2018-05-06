using CommonLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using static MyService.Service;

namespace MyService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        List<ImmoInfo> GetImmo();
        [OperationContract]
        ServiceOperationResult DBSave();
        [OperationContract]
        Immovables StartImmovablesEdit(int id);
        [OperationContract]
        Immovables SetImmovablesFieldValue(string fieldName, object val);
        [OperationContract]
        Immovables CancelEdit();
    }
}
