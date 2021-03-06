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
    [Table("AdminInfo")]
    public partial class AdminInfo : RecordBase<AdminInfo, int>
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LastLoginIP { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsValid { get; set; }
        public bool IsAdmin { get; set; }

		static AdminInfo ()
		{
			TableName = "AdminInfo";
			PrimaryKey = "Id";
			TableMappingInfo.Create(typeof(AdminInfo));
		}
    }
}
