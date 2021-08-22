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
    [Table("ELMAH_Error")]
    public partial class ELMAH_Error : RecordBase<ELMAH_Error, Guid>
    {
        [Key]
        public Guid ErrorId { get; set; }
        public string Application { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string User { get; set; }
        public int StatusCode { get; set; }
        public DateTime TimeUtc { get; set; }
        public int Sequence { get; set; }
        public string AllXml { get; set; }

		static ELMAH_Error ()
		{
			TableName = "ELMAH_Error";
			PrimaryKey = "ErrorId";
			TableMappingInfo.Create(typeof(ELMAH_Error));
		}
    }
}
