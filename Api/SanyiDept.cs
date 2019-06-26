using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using syDeptStatus.Models;
using SqlSugar;

namespace syDeptStatus.Api
{
    [Route("api/sy")]
    [ApiController]
    public class SanyiDeptController : ControllerBase
    {
        private readonly DbConnector con = new DbConnector();

        [HttpGet("cpstatics")]
        public string GetTheCpStatics(string CpDate)
        {
            var result = con._hisdb.Ado.SqlQueryDynamic(@"
            
            SELECT ( SELECT dept_name FROM dept_dict WHERE dept_code=T1.DEPT_CODE) dept_name,T1.SL in_sl, COUNT(1) discharge_sl
            FROM (SELECT COUNT(A.PATIENT_ID) SL, B.DEPT_CODE
                    FROM CP_ORDERS A, STAFF_DICT B
                    WHERE TO_CHAR(A.IN_DATE, 'yyyymm') = @1
                    AND A.INNER = B.EMP_NO
                    GROUP BY B.DEPT_CODE) T1,
                PAT_VISIT C
            WHERE T1.DEPT_CODE = C.DEPT_DISCHARGE_FROM
            AND TO_CHAR(C.DISCHARGE_DATE_TIME, 'yyyymm') = @1
            GROUP BY T1.SL, T1.DEPT_CODE
            ", new List<SugarParameter>
            {
                new SugarParameter("@1", CpDate) //时间格式: YYYYMM
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet("syrplstatic")]
        public string GetSyRplStatus(string SyDate)
        {
            var result = con._db.Ado.SqlQueryDynamic(@"
            SELECT *
            FROM V_SY_RPL_STATUS
            WHERE TO_CHAR(IMPORT_DATE_TIME, 'yyyymm') = @1
            ORDER BY FACT DESC
            ", new List<SugarParameter>
            {
                new SugarParameter("@1", SyDate)
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet("getissueamount")]
        public string GetIssueAmount()
        {
            var result = con._db.Ado.SqlQueryDynamic(@"
        SELECT COUNT(1) SL,
       CASE ISSUE_TYPE
         WHEN '1' THEN
          '药品异常'
         WHEN '2' THEN
          '耗材异常'
         WHEN '3' THEN
          '住院天数异常'
         WHEN '4' THEN
          '住院费用异常'
       END ISSUE_TYPE,
       TO_CHAR(IMPORT_DATE_TIME, 'yyyy-mm') IMPORT_DATE
        FROM SYLCODE.SY_ISSUES_MASTER 
        WHERE IMPORT_DATE_TIME>TO_DATE('2018-9-1','yyyy-mm-dd')
        GROUP BY ISSUE_TYPE, TO_CHAR(IMPORT_DATE_TIME, 'yyyy-mm')
        ORDER BY TO_CHAR(IMPORT_DATE_TIME, 'yyyy-mm'), ISSUE_TYPE
        ");

            return JsonConvert.SerializeObject(result);
        }


        // 获取lis的平均检查时间
        // 审核时间-医生请求时间
        [HttpGet("LisAvgData")]
        public string GetLisAvgData(string StartDate, string EndDate)
        {
            var result = con._lisdb.Ado.SqlQueryDynamic(@"
                SELECT patient_type, test_order_name, count(0) people_amount, trunc(AVG(costs_time),2) avgtime
                FROM 
                (
                SELECT CASE A.PATIENT_TYPE
                        WHEN '1' THEN
                            '住院'
                        WHEN '2' THEN
                            '门诊'
                        WHEN '3' THEN
                            '住院急诊'
                        WHEN '4' THEN
                            '门诊急诊'
                        END PATIENT_TYPE,
                        A.TEST_ORDER_NAME,
                        (A.CHECK_TIME - A.REQUISITION_TIME) * 24 COSTS_TIME
                    FROM LIS_INSPECTION_SAMPLE A
                WHERE A.REQUISITION_TIME >= TO_DATE(@1, 'yyyy-mm-dd')
                    AND A.REQUISITION_TIME < TO_DATE(@2, 'yyyy-mm-dd')
                    AND a.patient_type IN ('1','3')
                )
                GROUP BY patient_type, test_order_name
                ORDER BY test_order_name, patient_type
                ", new List<SugarParameter>
                {
                    new SugarParameter("@1", StartDate),
                    new SugarParameter("@2", EndDate)
                });
            return JsonConvert.SerializeObject(result);
        }
    }
}