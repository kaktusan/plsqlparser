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
using System.Collections.Generic;
using System.Text;

using Deveel.Data.DbSystem;

namespace Deveel.Data.Query {
	/// <summary>
	/// The node for fetching a table from the current transaction.
	/// </summary>
	/// <remarks>
	/// This is a tree node and has no children.
	/// </remarks>
	[Serializable]
	public class FetchTableNode : IQueryPlanNode {
		/// <summary>
		/// The name of the table to fetch.
		/// </summary>
		private readonly ObjectName tableName;

		/// <summary>
		/// The name to alias the table as.
		/// </summary>
		private readonly ObjectName aliasName;

		public FetchTableNode(ObjectName tableName, ObjectName aliasName) {
			this.tableName = tableName;
			this.aliasName = aliasName;
		}

		public virtual IList<ObjectName> DiscoverTableNames(IList<ObjectName> list) {
			if (!list.Contains(tableName))
				list.Add(tableName);

			return list;
		}

		/// <inheritdoc/>
		public virtual ITable Evaluate(IQueryContext context) {
			ITable t = context.GetTable(tableName);
			return aliasName != null ? new ReferenceTable((Table)t, aliasName) : t;
		}

		/// <inheritdoc/>
		public virtual IList<CorrelatedVariable> DiscoverCorrelatedVariables(int level, IList<CorrelatedVariable> list) {
			return list;
		}

		public void Explain(StringBuilder output) {
			throw new NotImplementedException();
		}

		public virtual string Name {
			get { return "FETCH: " + tableName + " AS " + aliasName; }
		}
	}
}