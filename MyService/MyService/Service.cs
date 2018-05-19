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
        PropertyInfo[] immoPropertyInfo;
        public Service()
        {
            GetPropInfo();
        }

        void GetPropInfo()
        {
            
            immoPropertyInfo = typeof(Immovables).GetProperties();
        }

        public EssenceSOR<Immovables> CancelEdit()
        {
            var operationResult = new EssenceSOR<Immovables>();
            try
            {
                var tempIr = new ImmoRepos();
                var d = tempIr.db.Immovables.FirstOrDefault(x => x.Id == immoEdit.Id);
                foreach (PropertyInfo myProp in immoPropertyInfo)
                {
                    string propName = myProp.Name;
                    foreach (PropertyInfo oldProp in immoPropertyInfo)
                    {
                        string oldpropName = oldProp.Name;
                        if (propName == oldpropName)
                        {
                            myProp.SetValue(immoEdit, oldProp.GetValue(d));
                        }
                    }
                }
                operationResult.Essence = immoEdit;
                return operationResult;
            }
            catch(Exception e)
            {
                operationResult.Message = e.Message;
                operationResult.IsSuccess = true;
            }
            return operationResult;
        }

        public ServiceOperationResult DBSave()
        {
            var operationResult = new ServiceOperationResult();
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

        public ListSOR<ImmoInfo> GetListOfNames()
        {
            var operationResult = new ListSOR<ImmoInfo>();
            try
            {
                var list = new List<ImmoInfo>();
                foreach (var immo in ir.Load())
                {
                    list.Add(new ImmoInfo(immo.Id, immo.Name));
                }
                operationResult.List = list;
            }
            catch (Exception e)
            {
                operationResult.Message = e.Message;
                operationResult.IsSuccess = true;
            }
            return operationResult;
        }
        public EssenceSOR<Immovables> SetImmovablesFieldValue(string fieldName, object val)
        {
            var operationResult = new EssenceSOR<Immovables>();
            try
            { 
                var prop = immoPropertyInfo.FirstOrDefault(myProp => myProp.Name == fieldName);
                var t = prop.PropertyType;
                if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (val == null)
                    {
                        operationResult.Message = "Поле нельзя оставить пустым";
                        operationResult.IsSuccess = true;
                        return operationResult;
                    }
                    t = Nullable.GetUnderlyingType(t);
                }
                prop.SetValue(immoEdit, Convert.ChangeType(val, t));
                operationResult.Essence = immoEdit;
            }
            catch(Exception e) 
            {        
                throw new FaultException($"{e.Message}");
            }
            return operationResult;
        }

        public EssenceSOR<Immovables> StartImmovablesEdit(int id)
        {
            var operationResult = new EssenceSOR<Immovables>();
            try
            {
                immoEdit = ir.LoadByID(id);
                operationResult.Essence = immoEdit;
            }
            catch(Exception e)
            {
                operationResult.Message = e.Message;
                operationResult.IsSuccess = true;
            }
            return operationResult;
        }
    }
}
