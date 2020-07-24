using System;
using System.Collections.Generic;
using System.Text;

namespace BistroApp.Models
{
    public static class DBQuery
    {
        public const string GET_CATEGORY_ORDER_ITEM = @"
                SELECT 
                sco.Id as SCOId, 
                c.Id as Category, 
                c.Name as CategoryName, 
                sc.Name as SubCategoryName, 
                oi.Id as OrderItemId, 
                oi.Name as OrderItemName, 
                oi.Description as Description, 
                sc.Id as SubCategory, 
                oi.Price
                FROM SubCategory_OrderItem sco 
                INNER JOIN SubCategory sc ON sc.Id = sco.SubCategoryId
                INNER JOIN OrderItem oi ON oi.Id = sco.OrderItemId
                INNER JOIN Category c ON c.Id = sc.CategoryId;";

        public const string GET_TABLES = @"SELECT Id, Name FROM Tables;";

        public const string GET_SUB_CATEGORIES = @"SELECT Id, Name FROM SubCategory;";

        public const string GET_ORDER_ITEMS = @"SELECT * FROM BistroDB.OrderItem;";

        public const string INSERT_ORDER_ITEM = @"INSERT INTO OrderItem(Name, Description, Price) VALUES (@Name, @Description, @Price);";

        public const string INSERT_SUB_CATEGORY_ORDER_ITEM = @"INSERT INTO SubCategory_OrderItem(SubCategoryId, OrderItemId) VALUES (@SubCategoryId, @OrderItemId);";

        public const string DELETE_SUB_CATEGORY_ORDER_ITEM = @"
            DELETE 
            SubCategory_OrderItem, 
            OrderItem 
            FROM SubCategory_OrderItem 
            INNER JOIN OrderItem ON OrderItem.Id = SubCategory_OrderItem.OrderItemId 
            WHERE SubCategory_OrderItem.Id = @SCOId 
            AND SubCategory_OrderItem.SubCategoryId = @SubCategoryId 
            AND SubCategory_OrderItem.OrderItemId = @OrderItemId;";

        public const string UPDATE_ORDER_ITEM = @"UPDATE OrderItem SET Name = @Name, Description = @Description, Price = @Price WHERE Id = @OrderItemId";

        public const string UPDATE_SUB_CATEGORY_ORDER_ITEM = @"
            UPDATE SubCategory_OrderItem 
            SET SubCategoryId = @SubCategoryId, OrderItemId = @OrderItemId 
            WHERE Id = @SubCategoryOrderItemId;";

        public const string GET_SUB_CATEGORY_ORDER_ITEM_ID = @"
            SELECT Id FROM SubCategory_OrderItem
            WHERE SubCategoryId = @SubCategoryId 
            AND OrderItemId = @OrderItemId;";

        public const string GET_TABLE_ORDER_ITEMS = @"
            SELECT
            toi.Id,
            toi.Quantity,
            (oi.Price * Quantity) AS TotalPrice,
            t.Id AS TableId,  
            t.Name AS TableName,
            oi.Id AS OrderItemId,
            oi.Name AS OrderItemName
            FROM Tables_OrderItem toi 
            INNER JOIN Tables t ON t.Id = toi.TableId 
            INNER JOIN OrderItem oi ON oi.Id = toi.OrderItemId;";

        public const string INSERT_TABLE_ORDERS_ITEM = @"INSERT INTO Tables_OrderItem(TableId, OrderItemId, Quantity) VALUES (@SelectedTable, @SelectedOrderItem, @Quantity);";

        public const string UPDATE_TABLE_ORDERS_ITEM = @"
            UPDATE Tables_OrderItem 
            SET Quantity = @Quantity
            WHERE TableId = @SelectedTable 
            AND OrderItemId = @SelectedOrderItem;";

        public const string DELETE_TABLE_ORDER = @"
            DELETE FROM Tables_OrderItem 
            WHERE TableId = @SelectedTable 
            AND OrderItemId = @SelectedOrderItem;";

        public const string INSERT_TABLE = @"INSERT INTO Tables (Name) VALUES (@Table)";

        public const string DELETE_TABLE = @"DELETE FROM Tables WHERE Id = @Table";

        public const string UPDATE_TABLE = @"UPDATE Tables SET Name = @TableName WHERE Id = @Table";
    }
}
