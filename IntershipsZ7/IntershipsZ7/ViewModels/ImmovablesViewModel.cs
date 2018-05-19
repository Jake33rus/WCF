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

namespace IntershipsZ7.ViewModels
{
    /// <summary>
    /// Вьюмодель окна, отображение списка сущностей, настройка свойств элементов формы
    /// </summary>
    public class ImmovablesViewModel:ChangeNotifier
    {
        /// <summary>
        /// проверка были ли изменены поля сущности
        /// </summary>
        static bool isChange = false;
        public bool IsChange { get { return isChange; }
            set
            {
                isChange = value;
                OnPropertyChanged();
            }
        }
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
        /// <summary>
        /// изменение доступности для нажатия кнопки "Repeal"
        /// </summary>
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

        /// <summary>
        /// видимость прогресс бара
        /// </summary>
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
        
        public ServiceClient client;
        /// <summary>
        /// хранит объекты которые будут показаны в ListView
        /// </summary>
        public ObservableCollection<ImmoInfo> ImmoObsCol { get; set; }
        /// <summary>
        /// // хранит выбранную сущность, которую вернул сервер 
        /// </summary>
        private Immovables selectedImmo;
        public Immovables SelectedImmo 
        {
            get { return selectedImmo; }
            set
            {
                selectedImmo = value;
                ImmoEditorVM = new ImmoEditorViewModel(SelectedImmo, client, this);
                OnPropertyChanged("SelectedType");
            }
        }
        /// <summary>
        /// хранит значение свойства SelectedItem объекта ListView 
        /// </summary>
        private ImmoInfo selectedListView;
        public ImmoInfo SelectedListView
        {
            get { return selectedListView; }
            set
            {
                IsSaveChanges();
                selectedListView = value;
                var info = client.StartImmovablesEdit(selectedListView.id);
                if (info.IsSuccess)
                {
                    MessageBox.Show("Произошла ошибка -> {0}", info.Message);
                }
                else
                {
                    SelectedImmo = info.Essence;

                }
                    
            }
        }

        /// <summary>
        /// команда вызываемая при нажатии на кнопку Save
        /// </summary>
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(obj =>
                    {
                        SaveChanged();
                    }));
            }
        }
        /// <summary>
        /// Сохраняет изменения в сущности
        /// </summary>
        private async void  SaveChanged()
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
                IsChange = false;
            }
            catch (Exception ex)
            {
                message = string.Format("Произошла ошибка:{0}", ex.Message);
                
            }
            finally
            {
                IsEnabledButton = true;
                IsPBVisible = false;
                MessageBox.Show(message, "MyApp");
            }
            
        }
        /// <summary>
        /// откатывает изменения в сущности
        /// </summary>
        private RelayCommand repealCommand;
        public RelayCommand RepealCommand
        {
            get
            {
                return repealCommand ??
                    (repealCommand = new RelayCommand(obj =>
                    {
                        var info = client.CancelEdit();
                        if (!info.IsSuccess)
                        {
                            SelectedImmo = info.Essence;
                            IsChange = false;
                            MessageBox.Show("Изменения отменены!", "MyApp");
                        }
                        else { MessageBox.Show("Произошла ошибка! -> {0}", info.Message); }
                    }));
            }
        }
        /// <summary>
        /// проверяет были ли изменения в сущности находящейся на редактировании
        /// </summary>
        public void IsSaveChanges()
        {
            if (!isChange)
                return;
           string msg = "Сохранить изменения?";
           MessageBoxResult result = MessageBox.Show(msg, "MyApp", MessageBoxButton.YesNo, MessageBoxImage.Warning);
           if (result == MessageBoxResult.Yes)
                {
                    SaveChanged();
                }
           if (result == MessageBoxResult.No)
                 {
                     var info = client.CancelEdit();
                     if (!info.IsSuccess)
                     {
                        SelectedImmo = info.Essence;
                        IsChange = false;
                        MessageBox.Show("Изменения отменены!", "MyApp");
                     }
                     else { MessageBox.Show("Произошла ошибка! -> {0}", info.Message); }
                     IsChange = false;
                  }
            
        }
        public ImmovablesViewModel()
        {
            client = new ServiceClient("WSHttpBinding_IService");
            GetImmovables();
        }
        
        public virtual void GetImmovables()
        { 
            ImmoObsCol = new ObservableCollection<ImmoInfo>();
            foreach (var item in client.GetListOfNames().List) 
             {
                ImmoObsCol.Add(item);
             }
        }       
    }

}
