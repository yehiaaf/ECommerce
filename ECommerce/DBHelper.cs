

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
            SqlConnection con = new SqlConnection(ConnectionString);

            con.Open();

            SqlCommand cmd = new SqlCommand(sql, con);

            cmd.CommandType = CommandType.Text;

            if (parameters != null) cmd.Parameters.AddRange(parameters);
            SqlDataReader reader = cmd.ExecuteReader();

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
                reader.Close();
                con.Close();
            }
            return tbl;
        }

     
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            
            con.Open();

            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
        }

     
        public static DataTable ExecuteStoredProcedure(string spName, params SqlParameter[] parameters)
        {
            SqlConnection con = new SqlConnection(ConnectionString);

            con.Open();

            SqlCommand cmd = new SqlCommand(spName, con);

            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null) cmd.Parameters.AddRange(parameters);

            SqlDataReader reader = cmd.ExecuteReader();

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
                reader.Close();
                con.Close();
            }

            return tbl;
        }

        public static int ExecuteStoredProcedureNonQuery(string spName, params SqlParameter[] parameters)
        {
            SqlConnection con = new SqlConnection(ConnectionString);

            con.Open();

            try
            {
                SqlCommand cmd = new SqlCommand(spName, con);

                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null) cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
        }

        
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            SqlConnection con = new SqlConnection(ConnectionString);

            con.Open();

            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                if (parameters != null) cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteScalar();
            }
            finally
            {
                con.Close();
            }
        }
    }
}
