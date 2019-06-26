using System;

namespace SY_ISSUES_REPLIES
{
    /// <summary>
    /// </summary>
    public class SY_ISSUES_REPLIES
    {
        /// <summary>
        ///     Desc:问题类型：1：药品。2：耗材。3：住院天数。4：住院费用（pk）
        ///     Default:
        ///     Nullable:False
        /// </summary>
        public string ISSUE_TYPE { get; set; }

        /// <summary>
        ///     Desc:保存时间（pk）
        ///     Default:
        ///     Nullable:False
        /// </summary>
        public DateTime SAVE_TIME { get; set; }

        /// <summary>
        ///     Desc:保存者工号
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string SAVER_EMP_NO { get; set; }

        /// <summary>
        ///     Desc:保存者姓名
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string SAVER_NAME { get; set; }

        /// <summary>
        ///     Desc:问题导入时间，对应主表问题导入时间。（pk）
        ///     Default:
        ///     Nullable:False
        /// </summary>
        public DateTime ISSUE_IMPORT_DATE_TIME { get; set; }

        /// <summary>
        ///     Desc:住院号（pk）
        ///     Default:
        ///     Nullable:False
        /// </summary>
        public string INP_NO { get; set; }

        /// <summary>
        ///     Desc:自查事实
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string FACT { get; set; }

        /// <summary>
        ///     Desc:自查结论
        ///     Default:
        ///     Nullable:True
        /// </summary>
        public string RESULT { get; set; }

        ///<summary>
        /// Desc:住院次数
        /// 
        ///</summary>
        public string VISIT_ID{get;set;}
    }
}