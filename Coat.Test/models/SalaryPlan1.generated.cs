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
    [Table("SalaryPlan1")]
    public partial class SalaryPlan1 : RecordBase<SalaryPlan1, int>
    {
        [Key]
        public int Id { get; set; }
        public double Tier1Range { get; set; }
        public double Tier2Range { get; set; }
        public double Tier3Range { get; set; }
        public double Tier4Range { get; set; }
        public double Tier5Range { get; set; }
        public double Tier1Rate { get; set; }
        public double Tier2Rate { get; set; }
        public double Tier3Rate { get; set; }
        public double Tier4Rate { get; set; }
        public double Tier5Rate { get; set; }

		static SalaryPlan1 ()
		{
			TableName = "SalaryPlan1";
			PrimaryKey = "Id";
			TableMappingInfo.Create(typeof(SalaryPlan1));
		}
    }
}