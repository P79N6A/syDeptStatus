using System;

namespace SY_ISSUES_MASTER
{
    /// <summary>
    /// </summary>
    public class SY_ISSUES_MASTER
    {
        /// <summary>
        ///     Desc:机构名称
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string HOSPITAL { get; set; }

        /// <summary>
        ///     Desc:科室名称(2018-9-17,位数40->60)
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string DEPT_NAME { get; set; }

        /// <summary>
        ///     Desc:住院号
        ///     Default:
        ///     Nullable:False
        /// </summary>
        public string INP_NO { get; set; }

        /// <summary>
        ///     Desc:
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string VISIT_ID { get; set; }

        /// <summary>
        ///     Desc:病人姓名
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string PATIENT_NAME { get; set; }

        /// <summary>
        ///     Desc:出院日期
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public DateTime? DISCHARGE_DATE_TIME { get; set; }

        /// <summary>
        ///     Desc:医生姓名
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string DOCTOR_NAME { get; set; }

        /// <summary>
        ///     Desc:总费用
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public double? TOTAL_COSTS { get; set; }

        /// <summary>
        ///     Desc:药品费用
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public double? DRUG_COSTS { get; set; }

        /// <summary>
        ///     Desc:药品OE值
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string DRUGOE { get; set; }

        /// <summary>
        ///     Desc:drgs名称
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string DRGS_NAME { get; set; }

        /// <summary>
        ///     Desc:最小下线
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public double? MIN_LIMIT { get; set; }

        /// <summary>
        ///     Desc:中位数
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public double? MID_LIMIT { get; set; }

        /// <summary>
        ///     Desc:最大上限
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public double? MAX_LIMIT { get; set; }

        /// <summary>
        ///     Desc:住院天数
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public short? HOSPITALIZATION_DAYS { get; set; }

        /// <summary>
        ///     Desc:超上限比
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public float? OVER_MAX_LIMIT_RATIO { get; set; }

        /// <summary>
        ///     Desc:主要诊断编码(2018-9-16, 位数10->20)
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string MAIN_DIAG_ICD_CODE { get; set; }

        /// <summary>
        ///     Desc:主要诊断名称
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string MAIN_DIAG_ICD_NAME { get; set; }

        /// <summary>
        ///     Desc:主要手术编码
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string MAIN_OPER_CODE { get; set; }

        /// <summary>
        ///     Desc:手术及操作名称
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string MAIN_OPER_NAME { get; set; }

        /// <summary>
        ///     Desc:疑似问题类型。1：药品。2：耗材。3：住院天数异常。4：住院费用异常
        ///     Default:
        ///     Nullable:False
        /// </summary>
        public string ISSUE_TYPE { get; set; }

        /// <summary>
        ///     Desc:导入数据的时间
        ///     Default:
        ///     Nullable:False
        /// </summary>
        public DateTime IMPORT_DATE_TIME { get; set; }

        /// <summary>
        ///     Desc:最终锁定无法反馈的时间
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public DateTime? FINAL_LOCK_TIME { get; set; }

        /// <summary>
        ///     Desc:耗材费用(2018-9-16添加)
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public double? CONSUMER_ITEMS_COSTS { get; set; }

        /// <summary>
        ///     Desc:耗材OE(2018-9-16添加)
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string CONSUMER_ITEMS_OE { get; set; }

        /// <summary>
        ///     Desc:主管医生
        /// </summary>
        public string DOCTOR_IN_CHARGE { get; set; }

        /// <summary>
        ///     Desc:转科
        /// </summary>
        public string TRANS_DEPT { get; set; }
    }
}