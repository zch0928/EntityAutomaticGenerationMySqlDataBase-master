using EntityCreateByMySql_zch.dao;
using EntityCreateByMySql_zch.entity;
using System.Collections.Generic;

namespace EntityCreateByMySql_zch.service
{
    class CreateService
    {
        CreateDao dao = new CreateDao();

        /**
         * 查询所有数据库
         */
        public List<string> queryAllDataBase(string connstr)
        {
            return dao.queryAllDataBase(connstr);
        }

        /**
         * 查询数据库中的所有数据表
         */
        public List<string> queryAllDataTable(string connstr, string dataBaseName)
        {
            return dao.queryAllDataTable(connstr,dataBaseName);
        }

        /**
         * 查询数据表结构
         */
        public List<TableStructure> queryTableStructure(string connstr, string tableName)
        {
            return dao.queryTableStructure(connstr,tableName);
        }

        /**
         * 创建C#文件内容
         */
        public string createFileContent(List<TableStructure> list, string nameSpace = "命名空间", string tableName = "表名")
        {
            return dao.createFileContent(list,nameSpace,tableName);
        }

        /**
         * 创建Java文件内容
         */
        public string createJavaFileContent(List<TableStructure> list, string package = "包名", string tableName = "表名")
        {
            return dao.createJavaFileContent(list,package,tableName);
        }
    }
}
