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
        ListSOR<ImmoInfo> GetListOfNames();
        [OperationContract]
        ServiceOperationResult DBSave();
        [OperationContract]
        EssenceSOR<Immovables> StartImmovablesEdit(int id);
        [OperationContract]
        EssenceSOR<Immovables> SetImmovablesFieldValue(string fieldName, object val);
        [OperationContract]
        EssenceSOR<Immovables> CancelEdit();
    }
}
