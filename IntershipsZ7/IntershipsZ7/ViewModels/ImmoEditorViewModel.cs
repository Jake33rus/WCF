using IntershipsZ7.MyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IntershipsZ7.ViewModels
{
    /// <summary>
    /// Вложенная модель отвечающая за выбранную сущность и ее редактирование 
    /// </summary>
    public class ImmoEditorViewModel:ChangeNotifier
    {  
        /// <summary>
        /// проверка были ли изменены поля сущности
        /// </summary>
        static bool isChange = false;
        public bool IsChange
        {
            get { return isChange; }
            set
            {
                isChange = value;
                OnPropertyChanged();
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
                        CancelEdit();
                    }));
            }
        }
        public void CancelEdit()
        {
            var info = client.CancelEdit();
            if (!info.IsSuccess)
            {
                ChangeableImmo = info.Essence;
                OnPropertyChanged("");
                IsChange = false;
                MessageBox.Show("Изменения отменены!", "MyApp");
            }
            else { MessageBox.Show("Произошла ошибка! -> {0}", info.Message); }
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
                CancelEdit();
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
        /// <summary>
        /// Сохраняет изменения в сущности
        /// </summary>
        private async void SaveChanged()
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
        private Immovables changeableImmo;
        public Immovables ChangeableImmo
        {
            get { return changeableImmo; }
            set
            {         
                changeableImmo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// проверка на программное заполнения значений полей 
        /// </summary>
        public ImmoEditorViewModel(Immovables immo, ServiceClient client, PropertyInfo[] property )
        {   

            this.client = client;
            ChangeableImmo = immo;
            GetTypeList();
        }
        ServiceClient client;
        /// <summary>
        /// List хранящий соотношение значения id и типов сущностей, служит для заполнения Combobox
        /// </summary>
        public List<RatioTypes> TypesList { get; set; }

        public string Name
        {
            get { return changeableImmo.Name; }
            set
            {
                Changed(value);
                OnPropertyChanged();
            }
        }

        public double Footage
        {
            get { return changeableImmo.Footage; }
            set
            {

                Changed(value);
                OnPropertyChanged();
            }
        }

        public string Location
        {
            get { return changeableImmo.Location; }
            set
            {
                if(Changed(value))
                OnPropertyChanged();
            }
        }

        public int Price
        {
            get { return changeableImmo.Price; }
            set
            {
                if(Changed(value))
                OnPropertyChanged();
            }
        }

        public int? NumbRooms
        {
            get { return changeableImmo.NumbRooms; }
            set
            {
                if(Changed(value))
                OnPropertyChanged();
            }
        }
    
        public string TypeApart
        {
            get { return changeableImmo.TypeApart; }
            set
            {
                if(Changed(value))
                OnPropertyChanged();
            }
        }

        public int? NumbFloors
        {
            get { return changeableImmo.NumbFloors; }
            set
            {
                if (Changed(value))
                OnPropertyChanged();
            }
        }

        public double? SizePlot
        {
            get { return changeableImmo.SizePlot; }
            set
            {
                if(Changed(value))
                OnPropertyChanged();
            }
        }

        public string Assigment
        {
            get { return changeableImmo.Assigment; }
            set
            {
                if(Changed(value))
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Значение свойства SelectedValue в комбобоксе показывающем тип сущности
        /// </summary>
        public virtual int SelectedType
        {
            get { return changeableImmo.Type; }
            set
            {
                if (Changed(value, "Type"))
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Заполняет лист значениями
        /// </summary>
        public void GetTypeList()
        {
            TypesList = new List<RatioTypes>();
            TypesList.Add(new RatioTypes() { Id = 2, TypeName = "NoLivingSpace" });
            TypesList.Add(new RatioTypes() { Id = 3, TypeName = "Apartments" });
            TypesList.Add(new RatioTypes() { Id = 4, TypeName = "Houses" });
            TypesList.Add(new RatioTypes() { Id = 5, TypeName = "LivingSpace" });
        }

        public bool Changed(object val, [CallerMemberName] string fieldName = "")
        {
            try
            {
                var info = client.SetImmovablesFieldValue(fieldName, val);
                if (info.IsSuccess)
                {
                    MessageBox.Show("Произошла ошибка -> {0}", info.Message);
                    IsChange = false;
                    return false;
                }
                else
                {
                    changeableImmo = info.Essence;
                    IsChange = true;
                    return true;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Ошибка - {exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


    }
}
}
