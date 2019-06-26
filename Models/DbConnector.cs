using SqlSugar;

namespace syDeptStatus.Models
{
    public class DbConnector
    {
        public SqlSugarClient _db = new SqlSugarClient(new ConnectionConfig
        {
            // ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.119.132)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl.localdomain)));User ID=system;Password=Passw0rd;",
            ConnectionString =
                "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.120.5.15)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));User ID=sylcode;Password=sylcode;",
            DbType = DbType.Oracle,
            IsAutoCloseConnection = true, //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
            InitKeyType = InitKeyType.SystemTable
        });

        public SqlSugarClient _hisdb = new SqlSugarClient(new ConnectionConfig
        {
            // ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.119.132)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl.localdomain)));User ID=system;Password=Passw0rd;",
            ConnectionString =
                "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.120.5.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));User ID=system;Password=apple9912;",
            DbType = DbType.Oracle,
            IsAutoCloseConnection = true, //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
            InitKeyType = InitKeyType.SystemTable
        });

        public SqlSugarClient _lisdb = new SqlSugarClient(new ConnectionConfig
        {
            ConnectionString =
                "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.120.5.5)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=lisdb)));User ID=dbo;Password=lisdb;",
            DbType=DbType.Oracle,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.SystemTable
    });
    }
}