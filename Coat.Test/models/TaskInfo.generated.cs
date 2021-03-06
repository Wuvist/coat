// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Coat.
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using Coat.Base;
using Coat.Base.SqlMapping;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace d2d
{
    [Table("TaskInfo")]
    public partial class TaskInfo : RecordBase<TaskInfo, int>
    {
        [Key]
        public int Id { get; set; }
        public DateTime FinishTime { get; set; }
        public string Memo { get; set; }
        public bool IsFinished { get; set; }
        public int Priority { get; set; }
        public bool IsHistory { get; set; }
        public bool Other { get; set; }
        public int? Device_Id { get; set; }
        public int? Job_Id { get; set; }
        public bool IsSMSSent { get; set; }
        public string SMSStatus { get; set; }
        public string TaskStatus { get; set; }
        public int? Rating { get; set; }
        public bool? HasSignature { get; set; }
        public DateTime? ETAStart { get; set; }
        public DateTime? ETAEnd { get; set; }

		static TaskInfo ()
		{
			TableName = "TaskInfo";
			PrimaryKey = "Id";
			TableMappingInfo.Create(typeof(TaskInfo));
		}
    }
}
