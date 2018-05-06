using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

using System.Reflection;
using System.Data.Entity;
using IntershipsZ7.RemoteService;
using IntershipsZ7.MyService;
using System.ServiceModel;

namespace IntershipsZ7.ViewModels
{
    class ImmovablesViewModel:ChangeNotifier
    {
        bool isChange = false;
        public bool IsChange { get { return isChange; }
            set
            {
                isChange = value;
                OnPropertyChanged();
            }
        }
        bool isProgrammingChange = false;
        bool isEnabledButton = true;
        public bool IsEnabledButton
        {
            get { return isEnabledButton; }
            set
            {
                isEnabledButton = value;
                OnPropertyChanged();
            }
        }
        bool isPBVisible;
        public bool IsPBVisible
        {
            get { return isPBVisible; }
            set
            {
                isPBVisible = value;
                OnPropertyChanged();
            }
        }
        
        ServiceClient client;
        public ObservableCollection<ImmoInfo> ImmoObsCol { get; set; }
        private int selectedType;
        public virtual int SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                Changed("Type", selectedType);
                OnPropertyChanged();
            }
        }
        private Immovables selectedImmo;
        public Immovables SelectedImmo
        {
            get { return selectedImmo; }
            set
            {
                isProgrammingChange = true;
                selectedImmo = value;
                selectedType = selectedImmo.Type;
                OnPropertyChanged("SelectedType");
                OnPropertyChanged();
                isProgrammingChange = false;
            }
        }
        private ImmoInfo selectedListView;
        public ImmoInfo SelectedListView
        {
            get { return selectedListView; }
            set
            {
                IsSaving();
                selectedListView = value;
                SelectedImmo = client.StartImmovablesEdit(selectedListView.id);
            }
        }

        public List<TypesViewModel> TypesList { get; set; }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(obj =>
                    {
                        Saver();
                    }));
            }
        }
        private async void Saver()
        {
            string message = null;
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    IsPBVisible = true;
                    var info = client.DBSave();
                    message = info.IsSuccess ? info.Message : "Изменения сохранены";
                });
                await task;
            }
            catch (Exception ex)
            {
                message = string.Format("Произошла ошибка:{0}", ex.Message);
            }
            IsEnabledButton = true;
            IsPBVisible = false;
            MessageBox.Show(message, "MyApp");
            IsChange = false;
        }

        private RelayCommand repealCommand;
        public RelayCommand RepealCommand
        {
            get
            {
                return repealCommand ??
                    (repealCommand = new RelayCommand(obj =>
                    {
                        SelectedImmo = client.CancelEdit();
                        IsChange = false;
                        MessageBox.Show("Изменения отменены!", "IntershipsZ8");
                    }));
            }
        }
        public void IsSaving()
        {
            if (isChange)
            {
                string msg = "Сохранить изменения?";
                MessageBoxResult result = MessageBox.Show(msg, "MyApp", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Saver();
                }
                if (result == MessageBoxResult.No)
                {
                    selectedImmo = client.CancelEdit();
                    IsChange = false;
                }
            }
        }
        public ImmovablesViewModel()
        {
            client = new ServiceClient("WSHttpBinding_IService");
            GetImmovables();
            GetTypeList(); 
        }
        
        public virtual void GetImmovables()
        { 
            ImmoObsCol = new ObservableCollection<ImmoInfo>();
            foreach (var item in client.GetImmo()) 
             {
                ImmoObsCol.Add(item);
             }
        }
        public void GetTypeList()
        {
            TypesList = new List<TypesViewModel>();
            TypesList.Add(new TypesViewModel() { Id = 2, TypeName = "NoLivingSpace" });
            TypesList.Add(new TypesViewModel() { Id = 3, TypeName = "Apartments" });
            TypesList.Add(new TypesViewModel() { Id = 4, TypeName = "Houses" });
            TypesList.Add(new TypesViewModel() { Id = 5, TypeName = "LivingSpace" });
        }
       

        public void Changed(string fieldName, object val)
        {
            if (!isProgrammingChange)
            {
                try
                {
                    if (val != "")
                    {
                        SelectedImmo = client.SetImmovablesFieldValue(fieldName, val);
                    }
                }
                catch(FaultException exception)
                {
                    MessageBox.Show($"Ошибка - {exception.Message}","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                IsChange = true;
            }
        }
    }

}
