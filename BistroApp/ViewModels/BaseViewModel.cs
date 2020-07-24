using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BistroApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool addButtonEnabled;
        private bool clearButtonEnabled;
        private bool updateButtonEnabled;
        private bool deleteButtonEnabled;
        private bool isEditMode;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual void LoadData() { }

        public NotificationManager NotificationManager { get; protected set; }

        public bool IsEditMode
        {
            get
            {
                return isEditMode;
            }
            set
            {
                SetProperty(ref isEditMode, value);
            }
        }

        public bool AddButtonEnabled
        {
            get
            {
                return addButtonEnabled;
            }
            set
            {
                SetProperty(ref addButtonEnabled, value);
            }
        }

        public bool ClearButtonEnabled
        {
            get
            {
                return clearButtonEnabled;
            }
            set
            {
                SetProperty(ref clearButtonEnabled, value);
            }
        }

        public bool UpdateButtonEnabled
        {
            get
            {
                return updateButtonEnabled;
            }
            set
            {
                SetProperty(ref updateButtonEnabled, value);
            }
        }

        public bool DeleteButtonEnabled
        {
            get
            {
                return deleteButtonEnabled;
            }
            set
            {
                SetProperty(ref deleteButtonEnabled, value);
            }
        }

        protected void Notify(string title, string message, NotificationType type)
        {
            NotificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = type,
            });
        }
    }
}
