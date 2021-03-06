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

using Deveel.Data.Sql.Expressions;

namespace Deveel.Data.Sql {
	[Serializable]
	public sealed class FilterExpression : ISqlElement, IPreparable {
		public FilterExpression(Expression expression) {
			if (expression == null)
				throw new ArgumentNullException("expression");

			Expression = expression;
		}

		public Expression Expression { get; private set; }

		public FilterExpression Prepare(IExpressionPreparer preparer) {
			return new FilterExpression(Expression.Prepare(preparer));
		}

		internal FilterExpression Append(Expression expression) {
			return new FilterExpression(Expression.And(Expression, expression));
		}

		void ISqlElement.ToString(ISqlWriter writer) {
			writer.Write(Expression);
		}

		object IPreparable.Prepare(IExpressionPreparer preparer) {
			return Prepare(preparer);
		}
	}
}