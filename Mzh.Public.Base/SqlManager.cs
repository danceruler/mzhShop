using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Base
{
    public class SqlManager
    {
        /// <summary>
        /// 准备弃用的方法
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        [Obsolete]
        public static DataTable FillDataTable(string connectString, string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 120;
                    cmd.CommandText = sql;
                    SqlDataAdapter dadFill = new SqlDataAdapter(cmd);
                    // dadFill.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    dadFill.Fill(dt);
                    return dt;
                }
            }catch(Exception ex)
            {
                Logger._.Error(ex.ToString());
                return null;
            }
            
        }

        [Obsolete]
        public static DataTable FillDataTable(string cmdText, string tablename, string connectionString)
        {
            DataTable dtReturn = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandTimeout = 120;
                cmd.CommandText = cmdText;
                SqlDataAdapter dadFill = new SqlDataAdapter(cmd);
                dadFill.Fill(dtReturn);
                dtReturn.TableName = tablename;
                conn.Close();
                conn.Dispose();
            }
            return dtReturn;
        }

        /// <summary>
        /// 准备弃用的方法
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        [Obsolete]
        public static bool ExecuteNonQueryWithSql(string connectString, string sql)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                SqlTransaction myTrans;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                myTrans = connection.BeginTransaction();
                cmd.Transaction = myTrans;

                try
                {
                    if (sql.Trim().Length > 1)
                    {
                        cmd.CommandText = sql;

                        cmd.ExecuteNonQuery();
                        myTrans.Commit();
                    }

                    return true;
                }
                catch (Exception e)
                {
                    if (myTrans != null)
                        myTrans.Rollback();
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 准备弃用的方法
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        [Obsolete]
        public static DataSet FillDataSet(string connectString, string sql)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandTimeout = 120;
                cmd.CommandText = sql;
                SqlDataAdapter dadFill = new SqlDataAdapter(cmd);
                //dadFill.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                dadFill.Fill(ds);
                return ds;
            }
        }

        /// <summary>
        /// 准备弃用的方法
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        [Obsolete]
        public static DataTable FillDataTable_AddWithKey(string connectString, string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandTimeout = 120;
                cmd.CommandText = sql;
                SqlDataAdapter dadFill = new SqlDataAdapter(cmd);
                dadFill.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                dadFill.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// 获取DataTable类型的数据源，用于一次性取回一张表
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="command">SqlCommand实例</param>
        /// <param name="tableName">表名，默认Table1</param>
        /// <param name="isAddWithKey">是否添加主键信息</param>
        /// <returns>DataTable类型的数据源</returns>
        public static DataTable FillDataTable(string connectString, SqlCommand command, string tableName = "Table1", bool isAddWithKey = false)
        {
            try
            {
                DataTable data = new DataTable();
                data.TableName = tableName;

                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    command.Connection = conn;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    if (isAddWithKey)
                        adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    adapter.Fill(data);
                    return data;
                }
            }
            catch (Exception exception)
            {
                Logger._.Error(exception.ToString());
                return null;
            }
        }

        /// <summary>
        /// 获取DataSet类型的数据源，用于一次性取回多张表
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="command">SqlCommand实例</param>
        /// <param name="isAddWithKey">是否添加主键信息</param>
        /// <returns></returns>
        public static DataSet FillDataSet(string connectString, SqlCommand command, bool isAddWithKey = false)
        {
            try
            {
                DataSet data = new DataSet();

                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    command.Connection = conn;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    if (isAddWithKey)
                        adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    adapter.Fill(data);
                    return data;
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 代表名的查询取回DataSet
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="command"></param>
        /// <param name="dicTableName"></param>
        /// <returns></returns>
        public static DataSet FillDataSetWithTableName(string connectString, SqlCommand command, Dictionary<string, string> dicTableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    command.Connection = conn;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    foreach (var item in dicTableName)
                    {
                        adapter.TableMappings.Add(item.Key, item.Value);
                    }

                    DataSet dst = new DataSet();
                    adapter.Fill(dst);
                    return dst;
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }


        public static bool BulkCopyAndExecuteNonQueryAfterSql(string connectString, DataSet ds, List<SqlCommand> cmds)
        {
            bool result = false;
            string error_sql = "";
            using (SqlConnection sqlConn = new SqlConnection(connectString))
            {

                SqlTransaction transaction = null;
                SqlBulkCopy bulkCopy = null;
                try
                {
                    sqlConn.Open();

                    transaction = sqlConn.BeginTransaction();
                    for (int n = 0; n < cmds.Count; n++)
                    {
                        cmds[n].Connection = sqlConn;
                        error_sql = cmds[n].CommandText;
                        cmds[n].Transaction = transaction;
                        cmds[n].ExecuteNonQuery();
                    }

                    bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.CheckConstraints, transaction);

                    foreach (DataTable oneDt in ds.Tables)
                    {
                        bulkCopy.ColumnMappings.Clear();
                        foreach (DataColumn dcPrepped in oneDt.Columns)
                            bulkCopy.ColumnMappings.Add(dcPrepped.ColumnName, dcPrepped.ColumnName);
                        bulkCopy.DestinationTableName = oneDt.TableName;
                        bulkCopy.BatchSize = oneDt.Rows.Count;
                        if (oneDt != null && oneDt.Rows.Count > 0)
                            bulkCopy.WriteToServer(oneDt);
                    }


                    transaction.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result = false;
                }
                finally
                {
                    sqlConn.Close();
                    if (bulkCopy != null)
                        bulkCopy.Close();
                }

            }

            return result;
        }

        public static bool BulkCopyAndCmds(string connectString, List<string> previousCmds, DataSet ds, List<string> nextCmds)
        {
            bool result = false;
            string error_sql = "";
            using (SqlConnection sqlConn = new SqlConnection(connectString))
            {

                SqlTransaction transaction = null;
                SqlBulkCopy bulkCopy = null;
                try
                {
                    sqlConn.Open();

                    transaction = sqlConn.BeginTransaction();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConn;
                    cmd.Transaction = transaction;

                    for (int n = 0; n < previousCmds.Count; n++)
                    {
                        cmd.CommandText = previousCmds[n];
                        error_sql = previousCmds[n];
                        cmd.ExecuteNonQuery();
                    }

                    bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.CheckConstraints, transaction);

                    foreach (DataTable oneDt in ds.Tables)
                    {
                        bulkCopy.ColumnMappings.Clear();
                        foreach (DataColumn dcPrepped in oneDt.Columns)
                            bulkCopy.ColumnMappings.Add(dcPrepped.ColumnName, dcPrepped.ColumnName);
                        bulkCopy.DestinationTableName = oneDt.TableName;
                        bulkCopy.BatchSize = oneDt.Rows.Count;
                        if (oneDt != null && oneDt.Rows.Count > 0)
                            bulkCopy.WriteToServer(oneDt);
                    }

                    for (int n = 0; n < nextCmds.Count; n++)
                    {
                        cmd.CommandText = nextCmds[n];
                        if (string.IsNullOrWhiteSpace(nextCmds[n]) == true)
                            continue;

                        error_sql = nextCmds[n];
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result = false;
                }
                finally
                {
                    sqlConn.Close();
                    if (bulkCopy != null)
                        bulkCopy.Close();
                }

            }

            return result;
        }




        /// <summary>
        /// 执行数据操作
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="command">SqlCommand实例</param>
        /// <param name="useTransaction">是否使用事务</param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string connectString, SqlCommand command, bool useTransaction = false)
        {
            SqlTransaction transaction = null;
            try
            {
                if (!useTransaction)
                {
                    using (SqlConnection conn = new SqlConnection(connectString))
                    {
                        conn.Open();
                        command.Connection = conn;
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(connectString))
                    {
                        conn.Open();
                        transaction = conn.BeginTransaction();
                        command.Connection = conn;
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (useTransaction && transaction != null)
                    transaction.Rollback();
                Logger._.Error("错误语句：" + command.CommandText + "\r\n" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 执行数据操作
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="command">SqlCommand实例</param>
        /// <param name="useTransaction">是否使用事务</param>
        /// <returns></returns>
        public static int ExecuteNonQueryCount(string connectString, SqlCommand command, bool useTransaction = false)
        {
            SqlTransaction transaction = null;
            int rs = 0;
            try
            {
                if (!useTransaction)
                {
                    using (SqlConnection conn = new SqlConnection(connectString))
                    {
                        conn.Open();
                        command.Connection = conn;
                        return command.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(connectString))
                    {
                        conn.Open();
                        transaction = conn.BeginTransaction();
                        command.Connection = conn;
                        command.Transaction = transaction;
                        rs = command.ExecuteNonQuery();
                        transaction.Commit();
                        return rs;
                    }
                }
            }
            catch (Exception ex)
            {
                if (useTransaction && transaction != null)
                    transaction.Rollback();
                return rs;
            }

            return rs;
        }

        /// <summary>
        /// 执行数据操作，用于多个业务，切勿用此方法执行批量插入操作
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="commands">SqlCommand实例集合</param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string connectString, List<SqlCommand> commands)
        {
            SqlTransaction transaction = null;
            SqlConnection conn = null;
            string sqlLog = string.Empty;

            try
            {
                using (conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (transaction = conn.BeginTransaction())
                    {
                        foreach (SqlCommand command in commands)
                        {
                            sqlLog = command.CommandText;
                            command.Connection = conn;
                            command.Transaction = transaction;
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }

                }
                return true;
            }
            catch (Exception ex)
            {

                if (transaction != null)
                    transaction.Rollback();
                if (conn != null)
                    conn.Close();
                return false;
            }
        }

        /// <summary>
        /// 批量插入单表数据
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="data">数据源，TableName要与数据库表名对应</param>
        /// <returns></returns>
        public static bool BulkInsert(string connectString, DataTable data)
        {
            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectString))
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
                    foreach (DataColumn column in data.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    bulkCopy.DestinationTableName = data.TableName;
                    bulkCopy.WriteToServer(data);
                    transaction.Commit();

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();

                return false;
            }
        }

        /// <summary>
        /// 执行多条SQL语句，返回是否执行成功
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public static bool ExecuteNonQueryWithSqlList(string connectString, List<string> SQLStringList)
        {
            string strsql = "";
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                SqlTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    Logger._.Error("错误语句："+ strsql+"\r\n"+e.Message);
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，返回是否执行成功
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public static bool ExecuteNonQueryWithSqlListAndTran(SqlTransaction tx, List<string> SQLStringList)
        {
            string strsql = "";
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = tx.Connection;
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    throw e;
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// 批量插入多表数据
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="data">数据源，TableName要与数据库表名对应</param>
        /// <returns></returns>
        public static bool BulkInsertWithTran(SqlTransaction tx, DataSet data)
        {
            SqlTransaction transaction = tx;
            try
            {
                {
                    foreach (DataTable table in data.Tables)
                    {
                        SqlBulkCopy bulkCopy = new SqlBulkCopy(tx.Connection, SqlBulkCopyOptions.Default, transaction);
                        foreach (DataColumn column in table.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.DestinationTableName = table.TableName;
                        bulkCopy.WriteToServer(table);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();

                return false;
            }
        }

        /// <summary>
        /// 批量插入多表数据
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="data">数据源，TableName要与数据库表名对应</param>
        /// <returns></returns>
        public static bool BulkInsert(string connectString, DataSet data)
        {
            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectString))
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    foreach (DataTable table in data.Tables)
                    {
                        SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
                        foreach (DataColumn column in table.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.DestinationTableName = table.TableName;
                        bulkCopy.WriteToServer(table);
                    }
                    transaction.Commit();

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();

                return false;
            }
        }

        public static void RunProcedure(string runProcedure, SqlParameter[] m_sps, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(runProcedure, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter parameter in m_sps)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        public static DataTable GetDataSetByProcedure(string runProcedure, SqlParameter[] m_sps, string connectionString)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(runProcedure, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter parameter in m_sps)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                    try
                    {
                        connection.Open();
                        new SqlDataAdapter(cmd).Fill(dt);
                        return dt;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }


        public static DataSet FillDataSet(string connectString, SortedList<string, string> sqls)
        {
            DataSet ds = new DataSet();
            foreach (string item in sqls.Keys)
            {
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 120;
                    cmd.CommandText = sqls[item];
                    SqlDataAdapter dadFill = new SqlDataAdapter(cmd);
                    //dadFill.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    dadFill.Fill(dt);
                }
                dt.TableName = item;
                if (dt.Columns.Contains("RI") && dt.Columns["RI"].DataType == typeof(Int64))
                {
                    dt.Columns["RI"].AutoIncrement = true;
                    dt.Columns["RI"].AutoIncrementSeed = -1;
                    dt.Columns["RI"].AutoIncrementStep = -1;
                }
                ds.Tables.Add(dt);
            }
            return ds;
        }

        public static bool ExecuteNonQueryWithTrans(SqlTransaction tx, List<SqlCommand> cmds)
        {
            string errText = string.Empty;
            try
            {

                foreach (SqlCommand oneCommand in cmds)
                {
                    oneCommand.Connection = tx.Connection;
                    oneCommand.Transaction = tx;
                    errText = oneCommand.CommandText;

                    oneCommand.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception e)
            {
                tx.Rollback();
                return false;
            }
            finally
            {
            }
        }

        /// <summary>
        /// 多数据库服务器事务提交
        /// </summary>
        /// <param name="mutilSqlLinks">键 连接字符串关键字, 值 Sql语句 </param>
        /// <returns>是否成功</returns>
        public static bool ExecuteMultiTran(Dictionary<string, List<string>> mutilSqlLinks)
        {
            bool reval = true;

            SqlCommand cmd = new SqlCommand();
            //事务对象名，事务对象的集合
            Dictionary<string, SqlTransaction> tranResult = new Dictionary<string, SqlTransaction>();

            //conn对象名，对象
            Dictionary<string, SqlConnection> connResult = new Dictionary<string, SqlConnection>();

            //当前是否执行成功
            bool isSuccess = true;

            List<string> keys = new List<string>();

            //通过connName进行循环执行事务
            foreach (KeyValuePair<string, List<string>> sqlLink in mutilSqlLinks)
            {
                string keyName = sqlLink.Key;

                //如果keys中已经存在当前 keyname，说明该conn的已经执行完毕，跳到下一keyname执行
                if (!keys.Contains(keyName))
                {
                    keys.Add(keyName);

                    string errText = string.Empty;

                    //提交当前conn的事务，如果失败，标记当前事务失败
                    try
                    {
                        SqlConnection conn = new SqlConnection(keyName);
                        conn.Open();
                        cmd.Connection = conn;
                        SqlTransaction tran = conn.BeginTransaction();
                        cmd.Transaction = tran;

                        //记录当前事务
                        tranResult.Add(keyName, tran);

                        //记录当前conn
                        connResult.Add(keyName, conn);

                        //读取当前conn的sql，执行
                        foreach (KeyValuePair<string, List<string>> sqlLinkItem in mutilSqlLinks)
                        {
                            if (sqlLinkItem.Key.Equals(keyName))
                            {
                                foreach (string sql in sqlLinkItem.Value)
                                {
                                    errText = sql;
                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }

                    if (!isSuccess)
                    {
                        break;
                    }
                }
            }

            //如果当前事务失败，把执行过的所有事务对象rollBack
            if (!isSuccess)
            {
                foreach (SqlTransaction sqlTran in tranResult.Values)
                {
                    sqlTran.Rollback();
                }
                reval = false;
            }
            else
            {
                foreach (SqlTransaction sqlTran in tranResult.Values)
                {
                    sqlTran.Commit();
                }
            }

            //关闭conn
            foreach (SqlConnection value in connResult.Values)
            {
                if (value.State != ConnectionState.Closed)
                {
                    value.Close();
                }
            }
            return reval;
        }

        /// <summary>
        /// 多数据库服务器事务提交
        /// </summary>
        /// <param name="mutilSqlLinks">键 连接字符串关键字, 值 Sql语句 </param>
        /// <returns>是否成功</returns>
        public static bool ExecuteMultiTran(Dictionary<string, List<SqlCommand>> mutilSqlCommands)
        {
            bool reval = true;

            //事务对象名，事务对象的集合
            Dictionary<string, SqlTransaction> tranResult = new Dictionary<string, SqlTransaction>();

            //conn对象名，对象
            Dictionary<string, SqlConnection> connResult = new Dictionary<string, SqlConnection>();

            //当前是否执行成功
            bool isSuccess = true;

            List<string> keys = new List<string>();

            //通过connName进行循环执行事务
            foreach (KeyValuePair<string, List<SqlCommand>> sqlCommands in mutilSqlCommands)
            {
                string keyName = sqlCommands.Key;

                //如果keys中已经存在当前 keyname，说明该conn的已经执行完毕，跳到下一keyname执行
                if (!keys.Contains(keyName))
                {
                    keys.Add(keyName);

                    string errText = string.Empty;

                    //提交当前conn的事务，如果失败，标记当前事务失败
                    try
                    {
                        SqlConnection conn = new SqlConnection(keyName);
                        conn.Open();
                        SqlTransaction tran = conn.BeginTransaction();

                        //记录当前conn
                        connResult.Add(keyName, conn);

                        //记录当前事务
                        tranResult.Add(keyName, tran);

                        //读取当前conn的sql，执行
                        foreach (KeyValuePair<string, List<SqlCommand>> sqlCommandsItem in mutilSqlCommands)
                        {
                            if (sqlCommandsItem.Key.Equals(keyName))
                            {
                                foreach (SqlCommand sqlCommand in sqlCommandsItem.Value)
                                {
                                    sqlCommand.Connection = conn;
                                    sqlCommand.Transaction = tran;
                                    errText = sqlCommand.CommandText;
                                    sqlCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }

                    if (!isSuccess)
                    {
                        break;
                    }
                }
            }

            //如果当前事务失败，把执行过的所有事务对象rollBack
            if (!isSuccess)
            {
                foreach (SqlTransaction sqlTran in tranResult.Values)
                {
                    sqlTran.Rollback();
                }
                reval = false;
            }
            else
            {
                foreach (SqlTransaction sqlTran in tranResult.Values)
                {
                    sqlTran.Commit();
                }
            }

            //关闭conn
            foreach (SqlConnection value in connResult.Values)
            {
                if (value.State != ConnectionState.Closed)
                {
                    value.Close();
                }
            }
            return reval;
        }
    }
}
