﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntershipsZ7.RemoteService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RemoteService.ISaver")]
    public interface ISaver {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISaver/DBSave", ReplyAction="http://tempuri.org/ISaver/DBSaveResponse")]
        string DBSave(CommonLib.Models.Immovables im);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISaver/DBSave", ReplyAction="http://tempuri.org/ISaver/DBSaveResponse")]
        System.Threading.Tasks.Task<string> DBSaveAsync(CommonLib.Models.Immovables im);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISaverChannel : IntershipsZ7.RemoteService.ISaver, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SaverClient : System.ServiceModel.ClientBase<IntershipsZ7.RemoteService.ISaver>, IntershipsZ7.RemoteService.ISaver {
        
        public SaverClient() {
        }
        
        public SaverClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SaverClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SaverClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SaverClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string DBSave(CommonLib.Models.Immovables im) {
            return base.Channel.DBSave(im);
        }
        
        public System.Threading.Tasks.Task<string> DBSaveAsync(CommonLib.Models.Immovables im) {
            return base.Channel.DBSaveAsync(im);
        }
    }
}
