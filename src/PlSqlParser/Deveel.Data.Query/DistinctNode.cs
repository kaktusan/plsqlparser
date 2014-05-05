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
using System.Text;

using Deveel.Data.DbSystem;

namespace Deveel.Data.Query {
	/// <summary>
	/// The node for performing a distinct operation on the given 
	/// columns of the child node.
	/// </summary>
	[Serializable]
	public class DistinctNode : SingleQueryPlanNode {
		/// <summary>
		/// The list of columns to be distinct.
		/// </summary>
		private readonly ObjectName[] columns;

		public DistinctNode(IQueryPlanNode child, ObjectName[] columns)
			: base(child) {
			this.columns = columns;
		}

		public override ITable Evaluate(IQueryContext context) {
			ITable t = Child.Evaluate(context);
			int sz = columns.Length;
			int[] colMap = new int[sz];
			for (int i = 0; i < sz; ++i) {
				colMap[i] = t.TableInfo.IndexOfColumn(columns[i]);
			}

			return t.Distinct(colMap);
		}

		public override string Name {
			get {
				StringBuilder sb = new StringBuilder();
				sb.Append("DISTINCT: (");
				for (int i = 0; i < columns.Length; ++i) {
					sb.Append(columns[i]);

					if (i < columns.Length - 1)
						sb.Append(", ");
				}
				sb.Append(")");
				return sb.ToString();
			}
		}
	}
}