using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace OnePlusYuanGong.dao
{
    class BaseDao
    {
        string connetStr = "";
        MySqlConnection conn = null;
        MySqlCommand cmd = null;
        MySqlDataReader reader = null;

        //设置连接字符串
        public void setConnectionStr(string str = null)
        {
            connetStr = str;
            if (str == null)
            {
                connetStr = "server=127.0.0.1;port=3306;user=root;password=123456;database=oneplus;";
            }
        }

        //获取连接
        public void getConnection()
        {
            conn = new MySqlConnection(connetStr);
            try
            {
                conn.Open();
            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //查询
        public MySqlDataReader query(string sql, string[] zd = null, object[] obj = null)
        {
            getConnection();
            cmd = new MySqlCommand(sql,conn);
            if (zd != null && obj != null
                && zd.Length == obj.Length)
            {
                for (int i = 0; i < zd.Length; i++)
                {
                    cmd.Parameters.AddWithValue(zd[i],obj[i]);
                }
            }
            reader = cmd.ExecuteReader();
            return reader;
        }

        // 增删改
        public int update(string sql, string[] zd = null, object[] obj = null)
        {
            getConnection();
            cmd = new MySqlCommand(sql, conn);
            if (zd != null && obj != null
                && zd.Length == obj.Length)
            {
                for (int i = 0; i < zd.Length; i++)
                {
                    cmd.Parameters.AddWithValue(zd[i], obj[i]);
                }
            }
            int count = cmd.ExecuteNonQuery();
            return count;
        }

        //关闭连接
        public void close(MySqlDataReader dr = null)
        {
            if (dr != null)
            {
                dr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }
    }
}
