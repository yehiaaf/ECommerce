

using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace ECommerceApp
{
    public static class DBHelper
    {
       
        public static readonly string ConnectionString =
            "Data Source=.;Initial Catalog=jinx;Integrated Security=True;Encrypt=False";

        
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            // 1- Instantiate the SqlConnection
            SqlConnection con = new SqlConnection(ConnectionString);

            // 2- Open the connection.
            con.Open();

            // 3- Instantiate a new command with a query and connection as parameters
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            if (parameters != null) cmd.Parameters.AddRange(parameters);

            // 4- Call Execute reader to get query results
            SqlDataReader reader = cmd.ExecuteReader();

            // To use the values from reader, we create DataTable
            DataTable tbl = new DataTable();

            // Add columns to the table, according to the columns in the reader
            for (int i = 0; i < reader.FieldCount; i++)
            {
                tbl.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }

            // To add a row to the table we use DataRow object
            DataRow row;
            try
            {
                while (reader.Read())
                {
                    // To ensure that the row has the same columns in the table, we use NewRow()
                    row = tbl.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader[i];
                    }
                    // Finally we add the row to the table
                    tbl.Rows.Add(row);
                }
            }
            finally
            {
                // 5- Close the reader and the connection
                reader.Close();
                con.Close();
            }

            return tbl;
        }

     
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            // 1- Instantiate the SqlConnection
            SqlConnection con = new SqlConnection(ConnectionString);

            // 2- Open the connection.
            con.Open();

            try
            {
                // 3- Instantiate a new command with a query and connection
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                if (parameters != null) cmd.Parameters.AddRange(parameters);

                // 4- Call ExecuteNonQuery to send command
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                // 5- Close Connection
                con.Close();
            }
        }

     
        public static DataTable ExecuteStoredProcedure(string spName, params SqlParameter[] parameters)
        {
            // 1- Instantiate the SqlConnection
            SqlConnection con = new SqlConnection(ConnectionString);

            // 2- Open the connection.
            con.Open();

            // 1. create a command object identifying the stored procedure
            SqlCommand cmd = new SqlCommand(spName, con);

            // 2. set the command object so it knows to execute a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // 3. add parameter to command, which will be passed to the stored procedure
            if (parameters != null) cmd.Parameters.AddRange(parameters);

            // 4. then you can execute the sp
            SqlDataReader reader = cmd.ExecuteReader();

            // Build DataTable manually 
            DataTable tbl = new DataTable();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                tbl.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }

            DataRow row;
            try
            {
                while (reader.Read())
                {
                    row = tbl.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader[i];
                    }
                    tbl.Rows.Add(row);
                }
            }
            finally
            {
                // 5- Close the reader and the connection
                reader.Close();
                con.Close();
            }

            return tbl;
        }

        // -----------------------------------------------------------------
        // Stored Procedure -> rows affected 
        // -----------------------------------------------------------------
        public static int ExecuteStoredProcedureNonQuery(string spName, params SqlParameter[] parameters)
        {
            // 1- Instantiate the SqlConnection
            SqlConnection con = new SqlConnection(ConnectionString);

            // 2- Open the connection.
            con.Open();

            try
            {
                // 1. create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand(spName, con);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                if (parameters != null) cmd.Parameters.AddRange(parameters);

                // 4. then you can execute the sp
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                // 5- Close Connection
                con.Close();
            }
        }

        
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            // 1- Instantiate the SqlConnection
            SqlConnection con = new SqlConnection(ConnectionString);

            // 2- Open the connection.
            con.Open();

            try
            {
                // 1. Instantiate a new command
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                if (parameters != null) cmd.Parameters.AddRange(parameters);

                // 2. Call ExecuteScalar to send command
                return cmd.ExecuteScalar();
            }
            finally
            {
                // 3- Close Connection
                con.Close();
            }
        }
    }
}
