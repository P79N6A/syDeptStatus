using System.Collections.Generic;
using SqlSugar;

namespace syDeptStatus.Models
{
    public class DbService
    {
        // SqlSugarClient _sugardb = new SqlSugarClient(new ConnectionConfig()
        // {
        //     ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.119.132)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl.localdomain)));User ID=system;Password=Passw0rd;",
        //     DbType = DbType.Oracle,
        //     InitKeyType = InitKeyType.SystemTable,
        //     IsAutoCloseConnection = true
        // });

        private readonly DbConnector connector = new DbConnector();

        public bool UserSignIn(string empno, string Password)
        {
            // var result = connector._db.Ado.GetInt("SELECT count(*) FROM STAFF_DICT@hislink WHERE emp_no=@1 AND PASSWORD=f_encript(@2)",
            var result = connector._db.Ado.GetInt(
                "SELECT count(*) FROM STAFF_DICT@hislink WHERE emp_no=@1 AND PASSWORD=f_encript(@2)",
                new List<SugarParameter>
                {
                    new SugarParameter("@1", empno),
                    new SugarParameter("@2", Password)
                });
            if (result == 0) return false;
            return true;
        }
    }
}