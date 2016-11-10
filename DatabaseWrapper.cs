using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace DatabaseWrapper
{
    /// <summary>
    /// MySQL Database Wrapper Class
    /// </summary>
    public class MySQLDatabaseWrapper
    {
        #region /* Fields */

        private string _connectionString = null;

        #endregion

        #region /* Constructors */

        public MySQLDatabaseWrapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region /* Methods */

        /// <summary>
        /// Execute stored procedure
        /// </summary>
        /// <param name="spName">Stored procedure name</param>
        /// <param name="arguments">Stored procedure arguments</param>
        /// <returns>Return Results as DataSet</returns>
        public DataSet ExecuteStoredProcedure(string spName, Dictionary<string, object> arguments)
        {
            DataSet result = new DataSet();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(spName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    //add command parameters
                    foreach(var pair in arguments)
                        command.Parameters.AddWithValue(pair.Key, pair.Value);

                    //execute SP
                    //We need MySqlDataAdapter to store all rows in the datatable
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(result);
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
