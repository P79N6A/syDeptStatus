using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using syDeptStatus.Models;
using SqlSugar;

namespace syDeptStatus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("any")]
    public class ValuesController : ControllerBase
    {
        // SqlSugarClient _db = new SqlSugarClient(new ConnectionConfig()
        // {
        //     ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.119.132)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl.localdomain)));User ID=system;Password=Passw0rd;",
        //     ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.120.5.15)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));User ID=sylcode;Password=sylcode;",
        //     DbType = DbType.Oracle,
        //     IsAutoCloseConnection = true,
        //     InitKeyType = InitKeyType.SystemTable
        // });

        // 初始化数据库连接。统一使用DbConnector
        private readonly DbConnector dbc = new DbConnector();

        // SqlSugarClient _db = new SqlSugarClient(new ConnectionConfig()
        // {
        //     ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.120.5.15)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));User ID=sylcode;Password=sylcode;",
        //     DbType = DbType.Oracle,
        //     IsAutoCloseConnection = true,
        //     InitKeyType = InitKeyType.SystemTable
        // });

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new [] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) { }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) { }

        // /// <summary>
        // /// 获取问题总表
        // /// </summary>
        // /// <returns></returns>
        // [HttpGet("issuemaster")]
        // public string IssuesMaster()
        // {
        //     var result = _db.Ado.SqlQueryDynamic("select * from V_SY_ISSUE_MASTER where final_lock_time>sysdate");
        //     var resultall = JsonConvert.SerializeObject(result);
        //     return resultall;
        // }

        /// <summary>
        ///     获取问题总表
        /// </summary>
        /// <returns></returns>
        [HttpGet("issuemaster")]
        public string IssuesMaster()
        {
            var result = dbc._db.Ado.SqlQueryDynamic(@"
                SELECT A.HOSPITAL,
                    A.DEPT_NAME,
                    A.INP_NO,
                    A.VISIT_ID,
                    A.PATIENT_NAME,
                    A.DISCHARGE_DATE_TIME,
                    A.DOCTOR_IN_CHARGE,
                    A.TOTAL_COSTS,
                    A.DRUG_COSTS,
                    A.DRUGOE,
                    A.DRGS_NAME,
                    A.MIN_LIMIT,
                    A.MID_LIMIT,
                    A.MAX_LIMIT,
                    A.HOSPITALIZATION_DAYS,
                    A.OVER_MAX_LIMIT_RATIO,
                    A.MAIN_DIAG_ICD_CODE,
                    A.MAIN_DIAG_ICD_NAME,
                    A.MAIN_OPER_CODE,
                    A.MAIN_OPER_NAME,
                    A.ISSUE_TYPE,
                    A.IMPORT_DATE_TIME,
                    A.FINAL_LOCK_TIME,
                    A.TRANS_DEPT,
                    COUNT(B.INP_NO) REPLIED_COUNT
                FROM V_SY_ISSUE_MASTER A, SY_ISSUES_REPLIES B
                WHERE A.FINAL_LOCK_TIME > SYSDATE
                    
                AND A.INP_NO = B.INP_NO(+)
                AND A.ISSUE_TYPE = B.ISSUE_TYPE(+)
                AND A.IMPORT_DATE_TIME = B.ISSUE_IMPORT_DATE_TIME(+)

                GROUP BY A.HOSPITAL,
                        A.DEPT_NAME,
                        A.INP_NO,
                        A.VISIT_ID,
                        A.PATIENT_NAME,
                        A.DISCHARGE_DATE_TIME,
                        A.DOCTOR_IN_CHARGE,
                        A.TOTAL_COSTS,
                        A.DRUG_COSTS,
                        A.DRUGOE,
                        A.DRGS_NAME,
                        A.MIN_LIMIT,
                        A.MID_LIMIT,
                        A.MAX_LIMIT,
                        A.HOSPITALIZATION_DAYS,
                        A.OVER_MAX_LIMIT_RATIO,
                        A.MAIN_DIAG_ICD_CODE,
                        A.MAIN_DIAG_ICD_NAME,
                        A.MAIN_OPER_CODE,
                        A.MAIN_OPER_NAME,
                        A.ISSUE_TYPE,
                        A.IMPORT_DATE_TIME,
                        A.FINAL_LOCK_TIME,
                        A.TRANS_DEPT
                ORDER BY A.DEPT_NAME
            ");
            // var jss = new JsonSerializerSettings();
            // jss.DateFormatString="yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            var resultall = JsonConvert.SerializeObject(result);
            return resultall;
        }

        /// <summary>
        ///     获取选择的病人的最后一条回复
        /// </summary>
        /// <param name="inp_no"></param>
        /// <param name="import_time"></param>
        /// <param name="issue_type"></param>
        /// <returns></returns>
        [HttpGet("getselectedlastcomment")]
        public string GetTheLastComment(string inpNo, string importTime, string issueType)
        {
            //            import_time= import_time.Replace('T',' ');
            importTime = importTime.Replace('T', ' ');
            var result = dbc._db.Ado.SqlQueryDynamic(@"
                SELECT * FROM (
                SELECT *
                FROM SY_ISSUES_REPLIES A
                WHERE A.INP_NO = @1
                AND A.ISSUE_IMPORT_DATE_TIME =
                    TO_DATE(@2, 'yyyy/mm/dd hh24:mi:ss')
                AND A.ISSUE_TYPE = @3
                ORDER BY a.SAVE_TIME DESC
                ) WHERE ROWNUM=1
                ", new List<SugarParameter>
            {
                new SugarParameter("@1", inpNo),
                new SugarParameter("@2", importTime),
                new SugarParameter("@3", issueType)
            });
            var strresult = JsonConvert.SerializeObject(result);
            return strresult;
        }

        [HttpGet("getselectedmaster")]
        public string GetTheSelectedIssueMaster(string inpNo, string importTime, string issueType, string visitId)
        {
            //            import_time= import_time.Replace('T',' ');
            importTime = importTime.Replace('T', ' ');
            var result = dbc._db.Ado.SqlQueryDynamic(@"
                SELECT *
                FROM V_SY_ISSUE_MASTER A
                WHERE A.INP_NO = @1
                AND A.IMPORT_DATE_TIME =
                    TO_DATE(@2, 'yyyy/mm/dd hh24:mi:ss')
                AND A.ISSUE_TYPE = @3
                AND A.VISIT_ID = @4

                ", new List<SugarParameter>
            {
                new SugarParameter("@1", inpNo),
                new SugarParameter("@2", importTime),
                new SugarParameter("@3", issueType),
                new SugarParameter("@4", visitId)
            });
            var strresult = JsonConvert.SerializeObject(result);
            return strresult;
        }

        [HttpPost("addreply")]
        public string AddTheReply(string issueType, string saverEmpNo, string saverName,
            string issueImportDateTime, string inpNo, string fact, string result)

        {
            issueImportDateTime = issueImportDateTime.Replace('T', ' ');
            var t1 = dbc._db.Ado.SqlQueryDynamic(@"
                INSERT INTO SY_ISSUES_REPLIES (ISSUE_TYPE,
                               SAVER_EMP_NO,
                               SAVER_NAME,
                               ISSUE_IMPORT_DATE_TIME,
                               INP_NO,
                               FACT,
                               RESULT)
                               VALUES (@1,@2,@3,to_date(@4,'yyyy-mm-dd hh24:mi:ss'),@5,@6,@7)",
                new List<SugarParameter>
                {
                    new SugarParameter("@1", issueType),
                    new SugarParameter("@2", saverEmpNo),
                    new SugarParameter("@3", saverName),
                    new SugarParameter("@4", issueImportDateTime),
                    new SugarParameter("@5", inpNo),
                    new SugarParameter("@6", fact),
                    new SugarParameter("@7", result)
                });

            return "添加成功";
        }

        [HttpPost("addreplyjson")]
        public string AddTheReplyJson(SY_ISSUES_REPLIES.SY_ISSUES_REPLIES obj)
        {
            string issueType, saverEmpNo, saverName, issueImportDateTime, inpNo, visiId, fact, result;
            issueType = obj.ISSUE_TYPE;
            saverEmpNo = obj.SAVER_EMP_NO;
            saverName = obj.SAVER_NAME;
            issueImportDateTime = obj.ISSUE_IMPORT_DATE_TIME.ToString().Replace('T', ' ');
            inpNo = obj.INP_NO;
            visiId = obj.VISIT_ID;
            fact = obj.FACT;
            result = obj.RESULT;

            var t1 = dbc._db.Ado.SqlQueryDynamic(@"
                    INSERT INTO SY_ISSUES_REPLIES (ISSUE_TYPE,
                               SAVER_EMP_NO,
                               SAVER_NAME,
                               ISSUE_IMPORT_DATE_TIME,
                               INP_NO,
                               FACT,
                               RESULT,
                               VISIT_ID)
                               VALUES (@1,@2,@3,to_date(@4,'yyyy-mm-dd hh24:mi:ss'),@5,@6,@7,@8)",
                new List<SugarParameter>
                {
                    new SugarParameter("@1", issueType),
                    new SugarParameter("@2", saverEmpNo),
                    new SugarParameter("@3", saverName),
                    new SugarParameter("@4", issueImportDateTime),
                    new SugarParameter("@5", inpNo),
                    new SugarParameter("@6", fact),
                    new SugarParameter("@7", result),
                    new SugarParameter("@8", visiId)
                });

            return "添加成功";
        }

        ///获取所选的历史评论。
        [HttpGet("GetHistoryRplies")]
        public string GetHistoryRplies(string inpNo, string issueImportDateTime, string issueType)
        {
            issueImportDateTime = issueImportDateTime.Replace('T', ' ');
            var result = dbc._db.Ado.SqlQueryDynamic(@"
            SELECT a.SAVE_TIME,a.SAVER_EMP_NO, a.SAVER_NAME, a.FACT, a.RESULT
            FROM SY_ISSUES_REPLIES A
            WHERE A.INP_NO = @1
            AND A.ISSUE_IMPORT_DATE_TIME =
                TO_DATE(@2, 'yyyy/mm/dd hh24:mi:ss')
            AND A.ISSUE_TYPE = @3
            ORDER BY save_time DESC
            ", new List<SugarParameter>
            {
                new SugarParameter("@1", inpNo),
                new SugarParameter("@2", issueImportDateTime),
                new SugarParameter("@3", issueType)
            });
            var resultall = JsonConvert.SerializeObject(result);
            return resultall;
        }

        //获取科室名称Group
        [HttpGet("GetDeptGroup")]
        public string GetDeptGroup()
        {
            // var result = dbc._db.Ado.SqlQueryDynamic (@"
            // SELECT DEPT_NAME
            // FROM V_SY_ISSUE_MASTER
            // WHERE FINAL_LOCK_TIME > SYSDATE
            // GROUP BY DEPT_NAME
            // ORDER BY dept_name ASC
            // ");

            var result = dbc._db.Ado.SqlQueryDynamic(@"
            SELECT DISTINCT regexp_substr(trans_dept,'[^,]+',1) trans_dept
            FROM V_SY_ISSUE_MASTER
            WHERE FINAL_LOCK_TIME > SYSDATE
            GROUP BY  trans_dept
            ORDER BY TRANS_DEPT ASC
            ");
            var resultStr = JsonConvert.SerializeObject(result);
            return resultStr;
        }

        [HttpGet("GetMasterByDept")]
        public string GetMasterByDept(string deptName)
        {
            var sqlstr = string.Format(@"
            SELECT A.HOSPITAL,
                    A.DEPT_NAME,
                    A.INP_NO,
                    A.VISIT_ID,
                    A.PATIENT_NAME,
                    A.DISCHARGE_DATE_TIME,
                    A.DOCTOR_IN_CHARGE,
                    a.ATTENDING_DOCTOR,
                    a.CHIEF_DOCTOR,
                    a.DIRECTOR,
                    A.TOTAL_COSTS,
                    A.DRUG_COSTS,
                    A.DRUGOE,
                    A.DRGS_NAME,
                    A.MIN_LIMIT,
                    A.MID_LIMIT,
                    A.MAX_LIMIT,
                    A.HOSPITALIZATION_DAYS,
                    A.OVER_MAX_LIMIT_RATIO,
                    A.MAIN_DIAG_ICD_CODE,
                    A.MAIN_DIAG_ICD_NAME,
                    A.MAIN_OPER_CODE,
                    A.MAIN_OPER_NAME,
                    A.ISSUE_TYPE,
                    A.IMPORT_DATE_TIME,
                    A.FINAL_LOCK_TIME,
                    A.TRANS_DEPT,
                    COUNT(B.INP_NO) REPLIED_COUNT
                FROM V_SY_ISSUE_MASTER A, SY_ISSUES_REPLIES B
                WHERE A.FINAL_LOCK_TIME > SYSDATE
                    
                AND A.INP_NO = B.INP_NO(+)
                AND A.ISSUE_TYPE = B.ISSUE_TYPE(+)
                AND A.IMPORT_DATE_TIME = B.ISSUE_IMPORT_DATE_TIME(+)
                AND A.TRANS_DEPT like '%{0}%'

                GROUP BY A.HOSPITAL,
                        A.DEPT_NAME,
                        A.INP_NO,
                        A.VISIT_ID,
                        A.PATIENT_NAME,
                        A.DISCHARGE_DATE_TIME,
                        A.DOCTOR_IN_CHARGE,
                        A.TOTAL_COSTS,
                        A.DRUG_COSTS,
                        A.DRUGOE,
                        A.DRGS_NAME,
                        A.MIN_LIMIT,
                        A.MID_LIMIT,
                        A.MAX_LIMIT,
                        A.HOSPITALIZATION_DAYS,
                        A.OVER_MAX_LIMIT_RATIO,
                        A.MAIN_DIAG_ICD_CODE,
                        A.MAIN_DIAG_ICD_NAME,
                        A.MAIN_OPER_CODE,
                        A.MAIN_OPER_NAME,
                        A.ISSUE_TYPE,
                        A.IMPORT_DATE_TIME,
                        A.FINAL_LOCK_TIME,
                        A.TRANS_DEPT,
                        a.ATTENDING_DOCTOR,
                        a.CHIEF_DOCTOR,
                        a.DIRECTOR
                ORDER BY A.DEPT_NAME
            
            ", deptName);
            // var result = dbc._db.Ado.SqlQueryDynamic (@"

            // ", new List<SugarParameter> () {
            //     new SugarParameter (":0", "%"+deptName+"%")
            // });

            var result = dbc._db.Ado.SqlQueryDynamic(sqlstr);
            var resultStr = JsonConvert.SerializeObject(result);
            return resultStr;
        }

        /// <summary>
        ///     三医监管，主任审核
        /// </summary>
        /// <param name="EmpNo">主任工号</param>
        /// <returns>返回主任所管科室的全部回复信息</returns>
        [HttpGet("GetChiefMaster")]
        public string GetChiefCheck(string EmpNo)
        {
            //         string sqlStr=string.Format(@"
            //         select * from sylcode.v_sy_rpl_status where 
            //     DEPT_DISCHARGE_FROM IN
            //    (SELECT AA.GROUP_CODE
            //       FROM STAFF_VS_GROUP@HISLINK AA
            //      WHERE AA.EMP_NO = '{0}')
            //         ", EmpNo
            //         );

            var result = dbc._db.Ado.SqlQueryDynamic(@"
            select * from sylcode.v_sy_rpl_status where 
                DEPT_DISCHARGE_FROM IN
                    (SELECT AA.GROUP_CODE
                        FROM STAFF_VS_GROUP@HISLINK AA
                        WHERE AA.EMP_NO = @empno)
            ", new SugarParameter("@empno", EmpNo));

            var resultStr = JsonConvert.SerializeObject(result);
            return resultStr;
        }

        /// <summary>
        ///     医保病种分值付费(大表，载入较慢)
        /// </summary>
        /// <param name="empNo">员工工号</param>
        /// <returns></returns>
        [HttpGet("sidgp")]
        public string SocialInsuranceDiseaseGroupPayment(string empNo)
        {
            JArray result = dbc._hisdb.Ado.SqlQueryDynamic(@"
            select * from sylcode.dss_grp_charge_detail where dept_discharge_from_code='030201'");
            return result.ToString();
        }
    }
}