using IntershipsZ7.MyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IntershipsZ7.Models
{
    public class ImmoModel
    {
        ServiceClient client;

        static PropertyInfo[] immoProperty;
        static ImmoModel()
        {
            immoProperty = typeof(Immovables).GetProperties(); 
        }
        
        public ImmoModel(ServiceClient client, Immovables immo)
        {
            this.client = client;
            Immo = immo;
        }

        public Immovables Immo { get; set; }

        public ServiceOperationResult ImmoSaveChanged()
        {
            return client.DBSave();
        }
        public EssenceSOROfImmovablesz2cTNpOZ ChangeField(string fieldName, object val)
        {
            return client.SetImmovablesFieldValue(fieldName, val);
        }
        public EssenceSOROfImmovablesz2cTNpOZ CancelEdit()
        {
            return client.CancelEdit();
        }
        public FieldModel GetField(string fieldName = "")
        {

            foreach (PropertyInfo prop in immoProperty)
            {
                if (prop.Name == fieldName)
                {
                    var temp = prop.GetValue(Immo);
                    if (temp != null)
                    {
                          return new FieldModel(temp.ToString(), fieldName, true, true);
                    }
                }
            }
            return new FieldModel(null, fieldName, false, false);
        }

    }
}
