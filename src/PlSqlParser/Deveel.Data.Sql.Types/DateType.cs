﻿// 
//  Copyright 2014  Deveel
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;

namespace Deveel.Data.Sql.Types {
	[Serializable]
	public sealed class DateType : DataType {
		public DateType(SqlType sqlType) 
			: base("DATE", sqlType) {
			AssertDateType(sqlType);
		}

		public static readonly string[] DateFormatSql = {
			"yyyy-MM-dd",
			"yyyy MM dd"
		};

		public static readonly string[] TimeFormatSql = {
			"HH:mm:ss.fff z",
			"HH:mm:ss.fff zz",
			"HH:mm:ss.fff zzz",
			"HH:mm:ss.fff",
			"HH:mm:ss z",
			"HH:mm:ss zz",
			"HH:mm:ss zzz",
			"HH:mm:ss"
		};

		public static readonly string[] TsFormatSql = {
			"yyyy-MM-dd HH:mm:ss.fff",
			"yyyy-MM-dd HH:mm:ss.fff z",
			"yyyy-MM-dd HH:mm:ss.fff zz",
			"yyyy-MM-dd HH:mm:ss.fff zzz",
			"yyyy-MM-dd HH:mm:ss",
			"yyyy-MM-dd HH:mm:ss z",
			"yyyy-MM-dd HH:mm:ss zz",
			"yyyy-MM-dd HH:mm:ss zzz",

			"yyyy-MM-ddTHH:mm:ss.fff",
			"yyyy-MM-ddTHH:mm:ss.fff z",
			"yyyy-MM-ddTHH:mm:ss.fff zz",
			"yyyy-MM-ddTHH:mm:ss.fff zzz",
			"yyyy-MM-ddTHH:mm:ss",
			"yyyy-MM-ddTHH:mm:ss z",
			"yyyy-MM-ddTHH:mm:ss zz",
			"yyyy-MM-ddTHH:mm:ss zzz",
		};


		private static void AssertDateType(SqlType sqlType) {
			if (sqlType != SqlType.Date &&
				sqlType != SqlType.Time &&
				sqlType != SqlType.TimeStamp)
				throw new ArgumentException(String.Format("The SQL type {0} is not a valid DATE", sqlType), "sqlType");
		}

		public override bool Equals(object obj) {
			var other = obj as DataType;
			if (other == null)
				return false;

			return SqlType == other.SqlType;
		}

		public override int GetHashCode() {
			return SqlType.GetHashCode();
		}
	}
}