﻿// 
//  Copyright 2010  Deveel
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
	/// A node for creating a table with a single row.
	/// </summary>
	/// <remarks>
	/// This table is useful for queries that have no underlying row. 
	/// For example, a pure functional table expression.
	/// </remarks>
	[Serializable]
	public class SingleRowTableNode : IQueryPlanNode {
		/// <inheritdoc/>
		public virtual IList<ObjectName> DiscoverTableNames(IList<ObjectName> list) {
			return list;
		}

		/// <inheritdoc/>
		public virtual ITable Evaluate(IQueryContext context) {
			// MILD HACK: Cast the context to a DatabaseQueryContext
			//DatabaseQueryContext dbContext = (DatabaseQueryContext)context;
			//return dbContext.Database.SingleRowTable;
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public virtual IList<CorrelatedVariable> DiscoverCorrelatedVariables(int level, IList<CorrelatedVariable> list) {
			return list;
		}

		public void Explain(StringBuilder output) {
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public virtual Object Clone() {
			return MemberwiseClone();
		}

		public virtual string Name {
			get { return "SINGLE ROW"; }
		}
	}
}