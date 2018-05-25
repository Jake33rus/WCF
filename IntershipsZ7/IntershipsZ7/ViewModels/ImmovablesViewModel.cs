using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Reflection;
using System.Data.Entity;
using IntershipsZ7.RemoteService;
using IntershipsZ7.MyService;
using System.ServiceModel;
using IntershipsZ7.Models;

namespace IntershipsZ7.ViewModels
{
    /// <summary>
    /// Вьюмодель окна, отображение списка сущностей, настройка свойств элементов формы
    /// </summary>
    public class ImmovablesViewModel:ChangeNotifier
    {
       
        private ImmoEditorViewModel immoEditorVM;
        public ImmoEditorViewModel ImmoEditorVM 
        {
            get { return immoEditorVM; }
            set
            {
                immoEditorVM = value;
                OnPropertyChanged();
            }
        }
        public ServiceClient client;
        /// <summary>
        /// хранит объекты которые будут показаны в ListView
        /// </summary>
        public ObservableCollection<ImmoInfo> ImmoName { get; set; }

        /// <summary>
        /// хранит значение свойства SelectedItem объекта ListView 
        /// </summary>
        private ImmoInfo selectedListView;
        public ImmoInfo SelectedListView
        {
            get { return selectedListView; }
            set
            {
                selectedListView = value;
                if (immoEditorVM != null)
                    immoEditorVM.IsSaveChanges();
                var info = client.StartImmovablesEdit(selectedListView.id);
                if (info.IsSuccess)
                {
                    MessageBox.Show("Произошла ошибка -> {0}", info.Message);
                }
                else
                {
                    ImmoModel immoModel = new ImmoModel(client, info.Essence); 
                    ImmoEditorVM = new ImmoEditorViewModel(immoModel);
                }              
            }
        }

        public void Closing()
        {
            immoEditorVM.IsSaveChanges();
        } 
        public ImmovablesViewModel()
        {
            client = new ServiceClient("WSHttpBinding_IService");
            GetImmovables();
        }
        
        public virtual void GetImmovables()
        { 
            ImmoName = new ObservableCollection<ImmoInfo>(client.GetListOfNames().List);
        }       
    }

}
