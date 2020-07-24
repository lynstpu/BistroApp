using BistroApp.Extensions;
using BistroApp.Models;
using Dapper;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace BistroApp.ViewModels
{
    public class TablesPageViewModel : BaseViewModel
    {
        private Tables selectedTable;
        private ICommand addNewCommand;
        private ICommand clearCommand;
        private ICommand updateCommand;
        private ICommand deleteCommand;
        private ObservableCollection<Tables> tables;
        private string tableName;

        public TablesPageViewModel()
        {
            NotificationManager = new NotificationManager();
        }

        public Tables SelectedTable
        {
            get
            {
                return selectedTable;
            }
            set
            {
                SetProperty(ref selectedTable, value);
                AddButtonEnabled = CanAdd();
                ClearButtonEnabled = CanClear();
                UpdateButtonEnabled = CanUpdate();
                DeleteButtonEnabled = CanDelete();
            }
        }

        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                SetProperty(ref tableName, value);
                AddButtonEnabled = CanAdd();
                ClearButtonEnabled = CanClear();
            }
        }

        public ICommand AddNewItemCommand
        {
            get
            {
                if (addNewCommand == null)
                {
                    addNewCommand = new RelayCommand<object>(param => AddNew(), param => CanAdd());
                }
                return addNewCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (clearCommand == null)
                {
                    clearCommand = new RelayCommand<object>(param => Clear(), param => CanClear());
                }
                return clearCommand;
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new RelayCommand<object>(param => Update(), param => CanUpdate());
                }
                return updateCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand<object>(param => Delete(), param => CanDelete());
                }
                return deleteCommand;
            }
        }

        public ObservableCollection<Tables> Tables
        {
            get
            {
                return tables;
            }
            set
            {
                SetProperty(ref tables, value);
            }
        }

        public void AddNew()
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Table", TableName);

                bool success = BistroDatabase.Instance.ExecuteQuery(DBQuery.INSERT_TABLE, parameters);

                AfterExecuteAction(success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Update()
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TableName", TableName);
                parameters.Add("@Table", SelectedTable.Id);
                bool success = BistroDatabase.Instance.ExecuteQuery(DBQuery.UPDATE_TABLE, parameters);
                AfterExecuteAction(success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected void Delete()
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Table", SelectedTable.Id);
                bool success = BistroDatabase.Instance.ExecuteQuery(DBQuery.DELETE_TABLE, parameters);
                AfterExecuteAction(success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected void Clear()
        {
            ResetFields();
        }

        private void AfterExecuteAction(bool success)
        {
            if (success)
            {
                LoadData();
                ResetFields();
                Notify("Info", "Action successfully executed!", NotificationType.Success);
            }
            else
            {
                Notify("Error", "Unable to execute action!", NotificationType.Error);
            }
        }

        private void ResetFields()
        {
            if (SelectedTable != null)
            {
                SelectedTable = null;
            }
            
            TableName = string.Empty;
            IsEditMode = false;
        }

        public bool CanAdd()
        {
            return !IsEditMode && !string.IsNullOrEmpty(TableName);
        }

        public bool CanClear()
        {
            return !string.IsNullOrEmpty(TableName) || SelectedTable != null;
        }

        public bool CanDelete()
        {
            return IsEditMode && SelectedTable != null;
        }

        public bool CanUpdate()
        {
            return IsEditMode && selectedTable != null;
        }

        public override void LoadData()
        {
            base.LoadData();
            Tables = BistroDatabase.Instance.Get<Tables>(DBQuery.GET_TABLES).ToObservableCollection();
        }
    }
}
