﻿using System;
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
    public class ImmovablesViewModel:ChangeNotifier
    {
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
        
        public ServiceClient client;
        public ObservableCollection<ImmoInfo> ImmoObsCol { get; set; }
        private Immovables selectedImmo;
        public Immovables SelectedImmo
        {
            get { return selectedImmo; }
            set
            {
                selectedImmo = value;
                ImmoEditorVM = new ImmoEditorViewModel(SelectedImmo, client, this);
                OnPropertyChanged("SelectedType");
                OnPropertyChanged();
            }
        }
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
