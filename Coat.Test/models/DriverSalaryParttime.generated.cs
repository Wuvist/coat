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
    [Table("DriverSalaryParttime")]
    public partial class DriverSalaryParttime : RecordBase<DriverSalaryParttime, int>
    {
        [Key]
        public int Id { get; set; }
        public int DeviceID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRegNo { get; set; }
        public string DriverName { get; set; }
        public string DriverIC { get; set; }
        public string DriverEmployeeID { get; set; }
        public string FileRefNo { get; set; }
        public string IssueDate { get; set; }
        public string PayFor { get; set; }
        public decimal EarningBasicWage { get; set; }
        public decimal EarningCommission { get; set; }
        public decimal EarningBonus { get; set; }
        public decimal EarningSubsidy { get; set; }
        public decimal EarningOthers { get; set; }
        public decimal DeductionRepair { get; set; }
        public decimal DeductionFine { get; set; }
        public decimal DeductionOther { get; set; }
        public decimal ClaimPetrol { get; set; }
        public decimal ClaimParking { get; set; }
        public decimal ClaimTopup { get; set; }
        public decimal ClaimServices { get; set; }
        public decimal? ClaimClaim { get; set; }
        public decimal? ClaimOther { get; set; }
        public string Bank { get; set; }
        public string BankBranch { get; set; }
        public string BankAccount { get; set; }
        public string Terms { get; set; }
        public string CheckNo { get; set; }
        public double? Cash { get; set; }
        public int? TotalWorkingDay { get; set; }
        public int? TotalItemsDelivered { get; set; }
        public int? DailyMaxDeliveryItem { get; set; }
        public double? DailyMaxDeliveryWeight { get; set; }
        public int? DailyMinDeliveryItem { get; set; }
        public double? DailyMinDeliveryWeight { get; set; }
        public double? AverageDailyItem { get; set; }
        public double? AverageItemWeight { get; set; }
        public int? Rating5 { get; set; }
        public int? Rating4 { get; set; }
        public int? Rating3 { get; set; }
        public int? Rating2 { get; set; }
        public int? Rating1 { get; set; }
        public int? CalculatePlan { get; set; }
        public string PlanRemark { get; set; }

		static DriverSalaryParttime ()
		{
			TableName = "DriverSalaryParttime";
			PrimaryKey = "Id";
			TableMappingInfo.Create(typeof(DriverSalaryParttime));
		}
    }
}