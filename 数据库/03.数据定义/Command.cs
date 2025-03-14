// *********************************************************************************
// # Project: SQLServer
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-11-25 19:11:54
// # Recently: 2025-02-11 00:02:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace JFramework.Net
{
    internal readonly struct Command
    {
        private readonly string database;

        public Command(string database)
        {
            this.database = database;
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            using var connection = new MySqlConnection(database);
            connection.Open();
            using var command = new MySqlCommand(query, connection);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            return command.ExecuteNonQuery();
        }

        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            using var connection = new MySqlConnection(database);
            connection.Open();
            using var command = new MySqlCommand(query, connection);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            using var adapter = new MySqlDataAdapter(command);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}