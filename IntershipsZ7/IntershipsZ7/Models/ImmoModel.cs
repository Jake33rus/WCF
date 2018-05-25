using IntershipsZ7.MyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntershipsZ7.Models
{
    public class ImmoModel
    {
        ServiceClient client;

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
    }
}
