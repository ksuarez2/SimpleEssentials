﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;

namespace CrackerBarrel.Foundation.DataStore
{
    public class DbStore : IDbStore
    {
        private string _connectionString { get; set; }

        public DbStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool Add<T>(T obj) where T : class, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();

                    long rowsAffected = connection.Insert(obj);
                    if (rowsAffected == 0)
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                //TODO: log this error
            }
            return true;
        }

        public int AddList<T>(IEnumerable<T> obj, string sql) where T : class, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    

                    return connection.Execute(sql, obj);
                }
            }
            catch (Exception ex)
            {
                return -1;
                //TODO: log this error
            }
        }
        
        public bool Delete<T>(T obj) where T : class, new()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();

                return connection.Delete(obj);
            }
        }

        public T Get<T>(object id) where T : class, new()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                return connection.Get<T>(id);
            }
        }

        public IEnumerable<T> GetByType<T>() where T : class, new()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();

                return connection.GetAll<T>();
            }
        }

        public IEnumerable<T> GetByParameters<T>(string sql, object param)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();

                return connection.Query<T>(sql, param);
            }
        }

        public IEnumerable<T> GetMultiMap<T, T2>(string sql, Func<T, T2, T> func, object param = null)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();

                return connection.Query<T, T2, T>(sql, func, param);

            }
        }

        public IEnumerable<T> GetMultiMap<T, T2, T3>(string sql, Func<T, T2, T3, T> func, object param = null)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();

                return connection.Query<T, T2, T3, T>(sql, func, param);

            }
        }

        public int Execute(string sql, object param)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    
                    return connection.Execute(sql, param);
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return -1;
                //TODO: log this error
            }
        }

        public bool Update<T>(T obj) where T : class, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    
                    return connection.Update(obj);
                }
            }
            catch (Exception ex)
            {
                return false;
                //TODO: log this error
            }
        }

        
    }
}