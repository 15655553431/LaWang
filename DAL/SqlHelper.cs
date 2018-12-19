using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class SqlHelper
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["lawangsql"].ToString();

        /// <summary>
        /// 执行查询语句或存储过程，返回第一行第一列数据或存储过程返回值
        /// </summary>
        /// <param name="cmdText">Sql语句或存储过程名称</param>
        /// <param name="ps">参数列表</param>
        /// <returns>第一行第一列数据或存储过程返回值</returns>
        public static object ExecuteScalar(string cmdText, CommandType type, params SqlParameter[] ps)
        {
            if (type == CommandType.Text)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                        {
                            if (ps != null)
                            {
                                cmd.Parameters.AddRange(ps);
                            }
                            conn.Open();
                            return cmd.ExecuteScalar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            else if (type == CommandType.StoredProcedure)
            {
                try
                {
                    for (int i = 0; i < ps.Length; i++)
                    {
                        if (ps[i].Value == null)
                        {
                            ps[i].Direction = ParameterDirection.Output;
                        }
                    }
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                        {
                            cmd.CommandType = type;
                            if (ps != null)
                            {
                                cmd.Parameters.AddRange(ps);
                            }
                            conn.Open();
                            cmd.ExecuteScalar();
                        }
                    }
                    List<Object> returnValue = new List<object>();
                    for (int i = 0; i < ps.Length; i++)
                    {
                        if (ps[i].Direction == ParameterDirection.Output)
                        {
                            returnValue.Add(ps[i].Value);
                        }
                    }
                    return returnValue;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 添加修改删除，带参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(ps);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(ps);

                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {


                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
    }
}
