using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using syDeptStatus.Models;
using SqlSugar;

namespace syDeptStatus.Controllers
{
    [Route("api/DocChief")]
    [ApiController]
    public class ValueDocChiefController
    {
        private readonly DbConnector dbc = new DbConnector();

        [HttpGet("GetSiMeasureByStaffDept")]
        public string GetSiMeasureByStaffDept()
        {
            var queryable = dbc._hisdb.Ado.GetDataTable(@"
            SELECT a.PATIENT_ID,
       a.VISIT_ID,
       a.BRFZ,
       a.SETTLING_DATE,
       ROUND(a.CALCU_SCORE,4) CALCU_SCORE,
       ROUND(a.OPERATING_GUIDE_CHARGE,4) OPERATING_GUIDE_CHARGE,
       ROUND(a.RATIFY_ALLOCATED_CHARGE,4) RATIFY_ALLOCATED_CHARGE,
       ROUND(a.PROFIT,4) PROFIT,
       b.dept_discharge_from,
       c.platform_name
  FROM SYLCODE.DSS_GRP_SI_MEASURE_DETAIL A, PAT_VISIT B, dept_dict c 
 WHERE A.PATIENT_ID = B.PATIENT_ID
   AND A.VISIT_ID = B.VISIT_ID
   AND b.dept_discharge_from=c.dept_code
   AND b.dept_discharge_from IN 
   (
       SELECT group_code FROM staff_vs_group WHERE emp_no=@1
   )
   
            ",
                new List<SugarParameter>
                {
                    new SugarParameter("@1", "1035")
                });
            var resultstr = JsonConvert.SerializeObject(queryable);
            return resultstr;
        }
    }
}