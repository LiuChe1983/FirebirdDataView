using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace FirebirdDataView
{
    class FbDataBase
    {
        private FbConnection cn;
        private string connectStr = "";
        public string lastError = "";

        public FbDataBase(string dataBasePath)
        {
            FbConnectionStringBuilder cs = new FbConnectionStringBuilder();
            cs.Database = dataBasePath;
            cs.UserID = "sysdba";
            cs.Password = "masterkey";
            cs.Charset = "UTF8";
            //cs.Dialect = 1;
            cs.ServerType = FbServerType.Embedded;
            connectStr = cs.ToString();

        }

        public bool Open()
        {
            cn = new FbConnection();
            cn.ConnectionString = connectStr;
            try
            {
                cn.Open();
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 关闭数据库连接，如果有
        /// </summary>
        public void Close()
        {
            if (cn == null) return;
            if (cn.State == ConnectionState.Open) cn.Close();
            cn.Dispose();
        }


        public bool ExecuteNonQuery(string sql)
        {
            FbCommand fcomm = new FbCommand(sql, cn);
            //cn.Open();
            try
            {
                fcomm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                lastError = ex.Message;

            }
            return false;
        }

        public DataTable GetDataTable(string tableName)
        {
            string sql =string.Format("SELECT * FROM {0}", tableName);
            return GetDataTableBySql(sql);
        }


        public DataTable GetDataTableBySql(string strSQL)
        {
            if ((cn == null) || (cn.State != ConnectionState.Open)) Open();

            DataTable dt1 = new DataTable();
            //string strSQL = "select * from TRADE where STATUS=3";//TRADE ORDERS
            ////string strSQL = "select * from TRADEEX";
            ////string strSQL = "SELECT COUNT(*) from ORDERS";//TRADE ORDERS

            ////SELECT COUNT(*) FROM
            FbDataAdapter fbDa = new FbDataAdapter(strSQL, cn);
            fbDa.Fill(dt1);
            return dt1;
        }

        public DataTable GetAllTable()
        {
            string sql = "SELECT RDB$RELATION_NAME AS TABLE_NAME FROM RDB$RELATIONS WHERE " +
                "RDB$SYSTEM_FLAG = 0 AND RDB$VIEW_SOURCE IS NULL;";
            DataTable dt_AllTable = GetDataTableBySql(sql);
            return dt_AllTable;
        }




    }
}
