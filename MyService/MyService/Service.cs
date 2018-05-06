using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CommonLib.Models;
using CommonLib.Repositories;

namespace MyService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    public class Service : IService
    {
        ImmoRepos ir = new ImmoRepos();
        
        Immovables immoEdit;
        ServiceOperationResult operationResult = new ServiceOperationResult();
        public Immovables CancelEdit()
        {
             var tempIr = new ImmoRepos();
             var d = tempIr.db.Immovables.FirstOrDefault(x => x.Id == immoEdit.Id);
             PropertyInfo[] myPropertyInfo = immoEdit.GetType().GetProperties();
             PropertyInfo[] oldPropertyInfo = d.GetType().GetProperties();
             foreach (PropertyInfo myProp in myPropertyInfo)
             {
                 string propName = myProp.Name;
                 foreach (PropertyInfo oldProp in oldPropertyInfo)
                 {
                     string oldpropName = oldProp.Name;
                     if (propName == oldpropName)
                     {
                         myProp.SetValue(immoEdit, oldProp.GetValue(d));
                     }
                 }
             }
            return immoEdit;
        }

        public ServiceOperationResult DBSave()
        {
            try
            {
                ir.Update(immoEdit.Id, immoEdit);
            }
            catch (Exception e)
            {
                operationResult.Message = e.Message;
                operationResult.IsSuccess = true;
            }
            return operationResult;
        }

        public List<ImmoInfo> GetImmo()
        {
            var list = new List<ImmoInfo>();
            foreach(var immo in ir.Load())
            {
                list.Add(new ImmoInfo(immo.Id, immo.Name));
            }
            return list;
        }

        public Immovables SetImmovablesFieldValue(string fieldName, object val)
        {
            PropertyInfo[] myPropertyInfo = immoEdit.GetType().GetProperties();
            var prop = myPropertyInfo.FirstOrDefault(myProp => myProp.Name == fieldName);
            var t = prop.PropertyType;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (val == null)
                {
                    return null;
                }
                t = Nullable.GetUnderlyingType(t);
            }
            try
            {
                prop.SetValue(immoEdit, Convert.ChangeType(val, t));
            }
            catch(Exception e) 
            {
                throw new FaultException("Введен не корректный символ, для данного поля\n");
            }
            return immoEdit;
        }

        public Immovables StartImmovablesEdit(int id)
        {
            immoEdit = ir.LoadByID(id);
            return immoEdit;
        }
    }
}
