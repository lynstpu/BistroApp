using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace BistroApp.Models
{
    public sealed class BistroDatabase
    {
        private static volatile BistroDatabase instance;
        private static readonly object SyncRoot = new object();

        public MySqlConnection Connection { get; }

        public BistroDatabase()
        {
            Connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        }

        public static BistroDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (instance == null)
                            instance = new BistroDatabase();
                    }
                }

                return instance;
            }
        }

        public IEnumerable<T> Get<T>(string query, DynamicParameters parameters = null)
        {
            return Connection.Query<T>(query, parameters);
        }

        public bool ExecuteQuery(string query, DynamicParameters parameters = null)
        {
            return Connection.Execute(query, parameters) > 0;
        }

        public bool OrderItemInsert(string query, string relationQuery, DynamicParameters parameters)
        {
            int inertedRowsCount = 0;
            Connection.Open();
            using MySqlTransaction transaction = Connection.BeginTransaction();
            try
            {
                int affectedRows = Connection.Execute(query, parameters, transaction: transaction);

                int orderItemId = Convert.ToInt32(Connection.ExecuteScalar<object>("SELECT LAST_INSERT_ID()", null, transaction: transaction));

                var subCategoryId = parameters.Get<int>("@SubCategoryId");

                var parms = new DynamicParameters();
                parms.Add("@SubCategoryId", parameters.Get<int>("@SubCategoryId"));
                parms.Add("@OrderItemId", orderItemId);

                inertedRowsCount = Connection.Execute(relationQuery, parms, transaction: transaction);
               
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
            }
            finally
            {
                Connection.Close();
            }

            return inertedRowsCount > 0;
        }

        public bool OrderItemUpdate(string query, string relationQuery, DynamicParameters parameters)
        {
            int uppdatedRowsCount = 0;
            Connection.Open();
            using MySqlTransaction transaction = Connection.BeginTransaction();
            try
            {
                int affectedRows = Connection.Execute(query, parameters, transaction: transaction);

                var parms = new DynamicParameters();
                parms.Add("@SubCategoryId", parameters.Get<int>("@SubCategoryId"));
                parms.Add("@OrderItemId", parameters.Get<int>("@OrderItemId"));
                parms.Add("@SubCategoryOrderItemId", parameters.Get<int>("@SubCategoryOrderItemId"));

                uppdatedRowsCount = Connection.Execute(relationQuery, parms, transaction: transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
            }
            finally
            {
                Connection.Close();
            }

            return uppdatedRowsCount > 0;
        }
    }
}
