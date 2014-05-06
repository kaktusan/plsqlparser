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
using System.Text;

using Deveel.Data.DbSystem;
using Deveel.Data.Sql.Expressions;

namespace Deveel.Data.Query {
	/// <summary>
	/// A branch node for a non-correlated ANY or ALL sub-query evaluation.
	/// </summary>
	/// <remarks>
	/// This node requires a set of columns from the left branch and an 
	/// operator.
	/// The right branch represents the non-correlated sub-query.
	/// <para>
	/// <b>Note:</b> The cost of a SubQuery is higher if the right child 
	/// result is greater than the left child result. The plan should be 
	/// arranged so smaller results are on the left.
	/// </para>
	/// </remarks>
	[Serializable]
	public class NonCorrelatedAnyAllNode : BranchQueryPlanNode {
		/// <summary>
		/// The columns in the left table.
		/// </summary>
		private readonly ObjectName[] leftColumns;

		/// <summary>
		/// The SubQuery operator, eg. '= ANY', '&lt;&gt; ALL'
		/// </summary>
		private readonly Operator subQueryOperator;

		public NonCorrelatedAnyAllNode(IQueryPlanNode left, IQueryPlanNode right, ObjectName[] leftColumns, Operator subQueryOperator)
			: base(left, right) {
			this.leftColumns = leftColumns;
			this.subQueryOperator = subQueryOperator;
		}

		public override ITable Evaluate(IQueryContext context) {
			// Solve the left branch result
			ITable leftResult = Left.Evaluate(context);
			// Solve the right branch result
			ITable rightResult = Right.Evaluate(context);

			// Solve the sub query on the left columns with the right plan and the
			// given operator.
			return leftResult.NonCorrelated(leftColumns, subQueryOperator, rightResult);
		}

		public override string Name {
			get {
				StringBuilder sb = new StringBuilder();
				sb.Append("NON_CORRELATED: (");
				for (int i = 0; i < leftColumns.Length; ++i) {
					sb.Append(leftColumns[i].ToString());
					if (i < leftColumns.Length - 1)
						sb.Append(", ");
				}
				sb.Append(") ");
				sb.Append(subQueryOperator.ToString());
				return sb.ToString();
			}
		}
	}
}