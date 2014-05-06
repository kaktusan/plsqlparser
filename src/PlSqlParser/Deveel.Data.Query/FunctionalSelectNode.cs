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

using Deveel.Data.DbSystem;
using Deveel.Data.Sql.Expressions;

namespace Deveel.Data.Query {
	/// <summary>
	/// The node for performing a functional select operation on the child node.
	/// </summary>
	/// <example>
	/// Some examples of this type of query are:
	/// <code>
	///   CONCAT(a, ' ', b) &gt; 'abba boh'
	///   TONUMBER(DATEFORMAT(a, 'yyyy')) &gt; 2001
	///   LOWER(a) &lt; 'ook'
	/// </code>
	/// The reason this is a separate node is because it is possible to exploit
	/// a functional indexes on a table with this node.
	/// <para>
	/// The given expression MUST be of the form:
	/// <code>
	///   'function_expression' 'operator' 'constant'
	/// </code>
	/// </para>
	/// </example>
	[Serializable]
	public class FunctionalSelectNode : SingleQueryPlanNode {
		/// <summary>
		/// The function expression (eg. CONCAT(a, ' ', b) == 'abba bo').
		/// </summary>
		private readonly Expression expression;

		public FunctionalSelectNode(IQueryPlanNode child, Expression expression)
			: base(child) {
			this.expression = expression;
		}

		public override ITable Evaluate(IQueryContext context) {
			ITable t = Child.Evaluate(context);
			// NOTE: currently this uses exhaustive select but should exploit
			//   function indexes when they are available.
			return t.ExhaustiveSelect(context, expression);
		}

		public override IList<ObjectName> DiscoverTableNames(IList<ObjectName> list) {
			return expression.DiscoverTableNames(base.DiscoverTableNames(list));
		}

		public override IList<CorrelatedVariable> DiscoverCorrelatedVariables(int level, IList<CorrelatedVariable> list) {
			return expression.DiscoverCorrelatedVariables(ref level,
					 base.DiscoverCorrelatedVariables(level, list));
		}
	}
}