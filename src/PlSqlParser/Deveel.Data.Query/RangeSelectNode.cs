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

using Deveel.Data.DbSystem;
using Deveel.Data.Expressions;
using Deveel.Data.Index;
using Deveel.Data.Types;

namespace Deveel.Data.Query {
	/// <summary>
	/// The node for performing a simple indexed query on a single column 
	/// of the child node.
	/// </summary>
	/// <remarks>
	/// Finds the set from the child node that matches the range.
	/// <para>
	/// The given <see cref="Expression"/> object must conform to a number of 
	/// rules. It may reference only one column in the child node. It must 
	/// consist of only simple mathemetical and logical operators (&lt;, &gt;, 
	/// =, &lt;&gt;, &gt;=, &lt;=, AND, OR).
	/// The left side of each mathematical operator must be a variable, 
	/// and the right side must be a constant (parameter subsitution or 
	/// correlated value).
	/// </para>
	/// <para>
	/// Breaking any of these rules will mean the range select can not 
	/// happen.
	/// </para>
	/// </remarks>
	/// <example>
	/// For example:
	/// <code>
	/// (col &gt; 10 AND col &lt; 100) OR col &gt; 1000 OR col == 10
	/// </code>
	/// </example>
	[Serializable]
	public class RangeSelectNode : SingleQueryPlanNode {
		/// <summary>
		/// A simple expression that represents the range to select.  See the
		/// class comments for a description for how this expression must be
		/// formed.
		/// </summary>
		private readonly Expression expression;

		public RangeSelectNode(IQueryPlanNode child, Expression expression)
			: base(child) {
			this.expression = expression;
		}

		/// <summary>
		/// Splits the given expression by the <i>and</i> operator.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="exp"></param>
		/// <remarks>
		/// For example, an expression of <c>a=9 and b=c and d=2</c> 
		/// would return the list: <c>a=9</c>,<c>b=c</c>, <c>d=2</c>.
		/// <para>
		/// If non <i>and</i> operators are found then the reduction stops.
		/// </para>
		/// </remarks>
		/// <returns>
		/// Returns a list of expressions that can be safely executed as 
		/// a set of <i>and</i> operations.
		/// </returns>
		private static IList<Expression> CreateAndList(IList<Expression> list, Expression exp) {
			return exp.BreakByOperator(list, Operator.And);
		}

		/// <summary>
		/// Updates a range with the given expression.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="range"></param>
		/// <param name="field"></param>
		/// <param name="e"></param>
		private static void UpdateRange(IQueryContext context, SelectableRangeSet range, DataColumnInfo field, Expression e) {
			var binary = (BinaryExpression) e;
			Operator op = binary.Operator;
			Expression[] exps = {binary.First, binary.Second};

			// Evaluate to an object
			DataObject cell = exps[1].Evaluate(context);

			// If the evaluated object is not of a comparable type, then it becomes
			// null.
			DataType fieldType = field.DataType;
			if (!cell.DataType.IsComparable(fieldType))
				cell = new DataObject(fieldType, null);

			// Intersect this in the range set
			range.Intersect(op, cell);
		}

		/// <summary>
		/// Calculates a list of <see cref="SelectableRange"/> objects that represent 
		/// the range of the expression.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="field"></param>
		/// <param name="range"></param>
		/// <param name="exp"></param>
		private static void CalcRange(IQueryContext context, DataColumnInfo field, SelectableRangeSet range, Expression exp) {
			var binary = (BinaryExpression) exp;
			Operator op = binary.Operator;

			if (op.IsLogical()) {
				if (op == Operator.And) {
					IList<Expression> andList = CreateAndList(new List<Expression>(), exp);
					foreach (Expression expr in andList) {
						UpdateRange(context, range, field, expr);
					}
				} else if (op == Operator.Or) {
					// Split left and right of logical operator.
					Expression[] exps = {binary.First, binary.Second};
					// Calculate the range of the left and right
					SelectableRangeSet left = new SelectableRangeSet();
					CalcRange(context, field, left, exps[0]);
					SelectableRangeSet right = new SelectableRangeSet();
					CalcRange(context, field, right, exps[1]);

					// Union the left and right range with the current range
					range.Union(left);
					range.Union(right);
				} else {
					throw new ApplicationException("Unrecognised logical operator.");
				}
			} else {
				// Not an operator so this is the value.
				UpdateRange(context, range, field, exp);
			}
		}

		public override ITable Evaluate(IQueryContext context) {
			ITable t = Child.Evaluate(context);

			Expression exp = expression;

			// Assert that all variables in the expression are identical.
			IEnumerable<ObjectName> allVars = exp.AllVariables();
			ObjectName v = null;
			foreach (ObjectName cv in allVars) {
				if (v != null && !cv.Equals(v))
					throw new ApplicationException("Assertion failed: Range plan does not contain common variable.");

				v = cv;
			}

			// Find the variable field in the table.
			int col = ((Table) t).FindFieldName(v);
			if (col == -1)
				throw new ApplicationException("Couldn't find column reference in table: " + v);

			DataColumnInfo field = t.TableInfo[col];
			// Calculate the range
			SelectableRangeSet range = new SelectableRangeSet();
			CalcRange(context, field, range, exp);

			// Select the range from the table
			SelectableRange[] ranges = range.ToArray();
			return t.RangeSelect(v, ranges);
		}

		/// <inheritdoc/>
		public override IList<ObjectName> DiscoverTableNames(IList<ObjectName> list) {
			return expression.DiscoverTableNames(base.DiscoverTableNames(list));
		}

		public override IList<CorrelatedVariable> DiscoverCorrelatedVariables(int level, IList<CorrelatedVariable> list) {
			//      Console.Out.WriteLine(expression);
			return expression.DiscoverCorrelatedVariables(ref level, base.DiscoverCorrelatedVariables(level, list));
		}

		public override string Name {
			get { return "RANGE: " + expression; }
		}
	}
}