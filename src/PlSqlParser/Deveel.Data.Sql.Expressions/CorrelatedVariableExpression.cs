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

using Deveel.Data.Query;

namespace Deveel.Data.Sql.Expressions {
	[Serializable]
	public sealed class CorrelatedVariableExpression : Expression {
		public CorrelatedVariableExpression(CorrelatedVariable correlatedVariable) {
			CorrelatedVariable = correlatedVariable;
		}

		public CorrelatedVariable CorrelatedVariable { get; private set; }

		public override ExpressionType ExpressionType {
			get { return ExpressionType.CorrelatedVariable; }
		}

		protected override DataObject OnEvaluate(IExpressionEvaluator evaluator) {
			return CorrelatedVariable.EvalResult;
		}
	}
}