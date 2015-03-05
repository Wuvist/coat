using System;
using System.Collections.Generic;
using System.Data;

namespace Coat.Base.SqlMapping
{
    public class TypeMap : Dictionary<Type, DbType>
    {
        private TypeMap()
        {
        }

        public static TypeMap Default()
        {
            var typeMap = new TypeMap();
            Type index1 = typeof(byte);
            int num1 = 2;
            typeMap[index1] = (DbType)num1;
            Type index2 = typeof(sbyte);
            int num2 = 14;
            typeMap[index2] = (DbType)num2;
            Type index3 = typeof(short);
            int num3 = 10;
            typeMap[index3] = (DbType)num3;
            Type index4 = typeof(ushort);
            int num4 = 18;
            typeMap[index4] = (DbType)num4;
            Type index5 = typeof(int);
            int num5 = 11;
            typeMap[index5] = (DbType)num5;
            Type index6 = typeof(uint);
            int num6 = 19;
            typeMap[index6] = (DbType)num6;
            Type index7 = typeof(long);
            int num7 = 12;
            typeMap[index7] = (DbType)num7;
            Type index8 = typeof(ulong);
            int num8 = 20;
            typeMap[index8] = (DbType)num8;
            Type index9 = typeof(float);
            int num9 = 15;
            typeMap[index9] = (DbType)num9;
            Type index10 = typeof(double);
            int num10 = 8;
            typeMap[index10] = (DbType)num10;
            Type index11 = typeof(Decimal);
            int num11 = 7;
            typeMap[index11] = (DbType)num11;
            Type index12 = typeof(bool);
            int num12 = 3;
            typeMap[index12] = (DbType)num12;
            Type index13 = typeof(string);
            int num13 = 16;
            typeMap[index13] = (DbType)num13;
            Type index14 = typeof(char);
            int num14 = 23;
            typeMap[index14] = (DbType)num14;
            Type index15 = typeof(Guid);
            int num15 = 9;
            typeMap[index15] = (DbType)num15;
            Type index16 = typeof(DateTime);
            int num16 = 6;
            typeMap[index16] = (DbType)num16;
            Type index17 = typeof(DateTimeOffset);
            int num17 = 27;
            typeMap[index17] = (DbType)num17;
            Type index18 = typeof(TimeSpan);
            int num18 = 17;
            typeMap[index18] = (DbType)num18;
            Type index19 = typeof(byte[]);
            int num19 = 1;
            typeMap[index19] = (DbType)num19;
            Type index20 = typeof(byte?);
            int num20 = 2;
            typeMap[index20] = (DbType)num20;
            Type index21 = typeof(sbyte?);
            int num21 = 14;
            typeMap[index21] = (DbType)num21;
            Type index22 = typeof(short?);
            int num22 = 10;
            typeMap[index22] = (DbType)num22;
            Type index23 = typeof(ushort?);
            int num23 = 18;
            typeMap[index23] = (DbType)num23;
            Type index24 = typeof(int?);
            int num24 = 11;
            typeMap[index24] = (DbType)num24;
            Type index25 = typeof(uint?);
            int num25 = 19;
            typeMap[index25] = (DbType)num25;
            Type index26 = typeof(long?);
            int num26 = 12;
            typeMap[index26] = (DbType)num26;
            Type index27 = typeof(ulong?);
            int num27 = 20;
            typeMap[index27] = (DbType)num27;
            Type index28 = typeof(float?);
            int num28 = 15;
            typeMap[index28] = (DbType)num28;
            Type index29 = typeof(double?);
            int num29 = 8;
            typeMap[index29] = (DbType)num29;
            Type index30 = typeof(Decimal?);
            int num30 = 7;
            typeMap[index30] = (DbType)num30;
            Type index31 = typeof(bool?);
            int num31 = 3;
            typeMap[index31] = (DbType)num31;
            Type index32 = typeof(char?);
            int num32 = 23;
            typeMap[index32] = (DbType)num32;
            Type index33 = typeof(Guid?);
            int num33 = 9;
            typeMap[index33] = (DbType)num33;
            Type index34 = typeof(DateTime?);
            int num34 = 6;
            typeMap[index34] = (DbType)num34;
            Type index35 = typeof(DateTimeOffset?);
            int num35 = 27;
            typeMap[index35] = (DbType)num35;
            Type index36 = typeof(TimeSpan?);
            int num36 = 17;
            typeMap[index36] = (DbType)num36;
            Type index37 = typeof(object);
            int num37 = 13;
            typeMap[index37] = (DbType)num37;
            return typeMap;
        }
    }
}
