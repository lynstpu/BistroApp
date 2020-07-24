using BistroApp.Extensions;
using BistroApp.Models;
using Dapper;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BistroApp.ViewModels
{
    public class OrdersPageViewModel : BaseViewModel
    {
        private List<Tables> tables = new List<Tables>() { new Tables { Id = 0, Name = "-- Select Table --" } };
        private List<OrderItem> orderItems = new List<OrderItem>() { new OrderItem { Id = 0, Name = "-- Select Item --" } };
        private Tables selectedTable;
        private OrderItem selectedOrderItem;
        private RelayCommand<object> addNewCommand;
        private RelayCommand<object> clearCommand;
        private RelayCommand<object> updateCommand;
        private RelayCommand<object> deleteCommand;
        private double quantity;
        private ObservableCollection<TableOrderItems> tableOrderItems;
        private bool tableDropdownEnabled = true;
        private bool orderItemsDropdownEnabled = true;

        public OrdersPageViewModel()
        {
            NotificationManager = new NotificationManager();
            SelectedTable = Tables.FirstOrDefault();
            SelectedOrderItem = OrderItems.FirstOrDefault();
        }

        public bool TableDropdownEnabled
        {
            get
            {
                return tableDropdownEnabled;
            }
            set
            {
                SetProperty(ref tableDropdownEnabled, value);
            }
        }

        public bool OrderItemsDropdownEnabled
        {
            get
            {
                return orderItemsDropdownEnabled;
            }
            set
            {
                SetProperty(ref orderItemsDropdownEnabled, value);
            }
        }

        public ObservableCollection<TableOrderItems> TableOrderItems
        {
            get
            {
                return tableOrderItems;
            }
            set
            {
                SetProperty(ref tableOrderItems, value);
            }
        }

        public List<Tables> Tables
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

        public List<OrderItem> OrderItems
        {
            get
            {
                return orderItems;
            }
            set
            {
                SetProperty(ref orderItems, value);
            }
        }

        public OrderItem SelectedOrderItem
        {
            get
            {
                return selectedOrderItem;
            }
            set
            {
                SetProperty(ref selectedOrderItem, value);
                AddButtonEnabled = CanAdd();
                ClearButtonEnabled = CanClear();
            }
        }

        public double Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                SetProperty(ref quantity, value);
                AddButtonEnabled = CanAdd();
                ClearButtonEnabled = CanClear();
            }
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

        public void AddNew()
        {
            try
            {
                bool success = false;
                var parameters = new DynamicParameters();
                parameters.Add("@SelectedTable", SelectedTable.Id);
                parameters.Add("@SelectedOrderItem", SelectedOrderItem.Id);

                bool existInOrders = TableOrderItems.Any(x => x.TableId == SelectedTable.Id && x.OrderItemId == SelectedOrderItem.Id);

                if (existInOrders)
                {
                    TableOrderItems existedItem = TableOrderItems.FirstOrDefault(x => x.TableId == SelectedTable.Id && x.OrderItemId == SelectedOrderItem.Id);
                    Quantity += existedItem.Quantity;
                    parameters.Add("@Quantity", Quantity);
                    success = BistroDatabase.Instance.ExecuteQuery(DBQuery.UPDATE_TABLE_ORDERS_ITEM, parameters);
                }
                else
                {
                    parameters.Add("@Quantity", Quantity);
                    success = BistroDatabase.Instance.ExecuteQuery(DBQuery.INSERT_TABLE_ORDERS_ITEM, parameters);
                }

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
                parameters.Add("@SelectedTable", SelectedTable.Id);
                parameters.Add("@SelectedOrderItem", SelectedOrderItem.Id);
                parameters.Add("@Quantity", Quantity);
                bool success = BistroDatabase.Instance.ExecuteQuery(DBQuery.UPDATE_TABLE_ORDERS_ITEM, parameters);
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
                parameters.Add("@SelectedTable", SelectedTable.Id);
                parameters.Add("@SelectedOrderItem", SelectedOrderItem.Id);
                bool success = BistroDatabase.Instance.ExecuteQuery(DBQuery.DELETE_TABLE_ORDER, parameters);
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
            Quantity = 0;
            SelectedTable = Tables.FirstOrDefault();
            SelectedOrderItem = OrderItems.FirstOrDefault();
            TableDropdownEnabled = true;
            OrderItemsDropdownEnabled = true;
            IsEditMode = false;
        }

        public bool CanAdd()
        {
            return Quantity > 0 && SelectedOrderItem.Id > 0 && SelectedTable.Id > 0 && !IsEditMode;
        }

        public bool CanClear()
        {
            return Quantity != 0 || SelectedTable != Tables.FirstOrDefault() || SelectedOrderItem != OrderItems.FirstOrDefault();
        }

        public bool CanDelete()
        {
            return IsEditMode && SelectedOrderItem.Id > 0 && SelectedTable.Id > 0;
        }

        public bool CanUpdate()
        {
            return IsEditMode && SelectedOrderItem.Id > 0;
        }

        public override void LoadData()
        {
            base.LoadData();
            TableOrderItems = BistroDatabase.Instance.Get<TableOrderItems>(DBQuery.GET_TABLE_ORDER_ITEMS).ToObservableCollection();
            IEnumerable<Tables> tables = BistroDatabase.Instance.Get<Tables>(DBQuery.GET_TABLES);
            IEnumerable<OrderItem> orderItems = BistroDatabase.Instance.Get<OrderItem>(DBQuery.GET_ORDER_ITEMS);
            Tables.AddRange(tables);
            OrderItems.AddRange(orderItems);
        }
    }
}
