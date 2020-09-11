using EntityCreateByMySql_zch.entity;
using MySql.Data.MySqlClient;
using OnePlusYuanGong.dao;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EntityCreateByMySql_zch.dao
{
    class CreateDao : BaseDao
    {
        /**
         * 查询所有数据库
         */
        public List<string> queryAllDataBase(string connstr)
        {
            String sql = "SHOW DATABASES WHERE `Database` NOT IN ('information_schema','mysql','performance_schema')";
            base.setConnectionStr(connstr);
            MySqlDataReader dr = base.query(sql);
            List<string> list = new List<string>();
            try
            {
                while (dr.Read())
                {
                    list.Add(dr.GetString("Database"));
                }
                if (list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                base.close(dr);
            }
        }

        /**
         * 查询数据库中的所有数据表
         */
        public List<string> queryAllDataTable(string connstr,string dataBaseName)
        {
            string sql = "SELECT table_name FROM information_schema.tables WHERE table_schema=@name;";
            string[] zd = {"@name"};
            object[] obj = {dataBaseName};
            base.setConnectionStr(connstr);
            MySqlDataReader dr = base.query(sql,zd,obj);
            List<string> list = new List<string>();
            try
            {
                while (dr.Read())
                {
                    list.Add(dr.GetString("table_name"));
                }
                if (list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                base.close(dr);
            }
        }

        /**
         * 查询数据表结构
         */
        public List<TableStructure> queryTableStructure(string connstr,string tableName)
        {
            String sql = "DESC `"+tableName+ "`";
            base.setConnectionStr(connstr);
            MySqlDataReader dr = base.query(sql);
            List<TableStructure> list = new List<TableStructure>();
            try
            {
                while (dr.Read())
                {
                    TableStructure ts = new TableStructure(dr.GetString("Field"),dataTypeDiscriminate(dr.GetString("Type")));
                    list.Add(ts);
                }
                if (list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                base.close(dr);
            }
        }

        /**
         * 数据类型识别
         */
        public string dataTypeDiscriminate(string type)
        {
            string leix = "";
            if (type.IndexOf("(") != -1)
            {
                leix = type.Substring(0,type.IndexOf("("));
            }
            switch (leix)
            {
                case "int":
                    return "int";
                case "decimal":
                    return "double";
                case "numeric":
                    return "double";
                case "float":
                    return "float";
                default:
                    return "string";
            }
        }

        /**
         * 创建C#文件内容
         */
        public string createFileContent(List<TableStructure> list,string nameSpace="命名空间",string tableName = "表名")
        {
            string ziduan = "";
            string getAndSet = "";
            string gouZao1 = "";
            string gouZao2 = "";
            foreach (TableStructure item in list)
            {
                string ffName = "";
                ziduan += "\t\tprivate " + item.Type1 + " " + item.Field1 + ";\n";
                if (Regex.IsMatch(item.Field1.Substring(0, 1), "[A-Z]"))
                {
                    ffName = item.Field1 + "1";
                }
                else
                {
                    ffName = item.Field1.Substring(0, 1).ToUpper() + item.Field1.Substring(1);
                }
                getAndSet += "\n\t\tpublic "+item.Type1+" " + ffName + " { get => " + item.Field1 + "; set => " + item.Field1 + " = value; }\n";
            }
            gouZao1 = "\n\t\tpublic "+tableName+"()\n\t\t{\n\t\t}\n";
            string gouZaoZiDuan = "";
            string gouZaoZiDuanFuZhi = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    gouZaoZiDuan += list[i].Type1 + " " + list[i].Field1 + "zch";
                    gouZaoZiDuanFuZhi += "\n\t\t\t" + list[i].Field1 + " = " + list[i].Field1 + "zch;";
                }
                else
                {
                    gouZaoZiDuan += "," + list[i].Type1 + " " + list[i].Field1 + "zzy";
                    gouZaoZiDuanFuZhi += "\n\t\t\t" + list[i].Field1 + " = " + list[i].Field1 + "zzy;";
                }
            }
            gouZao2 = "\n\t\tpublic " + tableName + "(" + gouZaoZiDuan + ")\n\t\t{" + gouZaoZiDuanFuZhi + "\n\t\t}\n";
            string str = "namespace " + nameSpace +
                         "\n{" +
                         "\n\tclass " + tableName + "" +
                         "\n\t{" +
                         "\n" + ziduan + gouZao1 + gouZao2 + getAndSet + "\t}\n}";
            return str;
        }

        /**
         * 创建Java文件内容
         */
        public string createJavaFileContent(List<TableStructure> list, string package = "包名", string tableName = "表名")
        {
            string ziduan = "";
            string getAndSet = "";
            foreach (TableStructure item in list)
            {
                string ffName = "";
                ziduan += "\tprivate " + item.Type1 + " " + item.Field1 + ";\n";
                if (Regex.IsMatch(item.Field1.Substring(0, 1), "[A-Z]"))
                {
                    ffName = item.Field1 + "1";
                }
                else
                {
                    ffName = item.Field1.Substring(0, 1).ToUpper() + item.Field1.Substring(1);
                }
                getAndSet += "\n\tpublic "+item.Type1+" get" + item.Field1.Substring(0, 1).ToUpper() + item.Field1.Substring(1) + "(){\n\t\treturn " + item.Field1 + ";\n\t}" +
                    "\n\tpublic void set" + item.Field1.Substring(0, 1).ToUpper() + item.Field1.Substring(1) + "(" + item.Type1 + " " + item.Field1 + "){\n\t\tthis." + item.Field1 + " = " + item.Field1 + ";\n\t}";
            }
            string gouZao1 = "\n\tpublic " + tableName + "(){\n\t\tsuper();\n\t}\n";
            string gouZaoZiDuan = "";
            string gouZaoZiDuanFuZhi = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0)
                {
                    gouZaoZiDuan += ",";
                }
                gouZaoZiDuan += list[i].Type1 + " " + list[i].Field1;
                gouZaoZiDuanFuZhi += "\n\t\tthis." + list[i].Field1 + " = " + list[i].Field1 + ";";
            }
            string gouZao2 = "\tpublic " + tableName + "(" + gouZaoZiDuan + "){\n\t\tsuper();" + gouZaoZiDuanFuZhi + "\n\t}\n";
            string str = "package "+package+";" +
                         "\npublic class "+tableName+"{" +
                         "\n" + ziduan + getAndSet + gouZao1 + gouZao2 + "}";
            return str;
        }
    }
}