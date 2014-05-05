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

namespace Deveel.Data.Expressions {
	public sealed class FunctionCallExpression : Expression {
		public FunctionCallExpression(ObjectName functionName, IEnumerable<Expression> arguments) 
			: this(null, functionName, arguments) {
		}

		public FunctionCallExpression(Expression obj, ObjectName functionName, IEnumerable<Expression> arguments) {
			Arguments = arguments;
			FunctionName = functionName;
			Object = obj;
		}

		public Expression Object { get; private set; }

		public ObjectName FunctionName { get; private set; }

		public IEnumerable<Expression> Arguments { get; private set; }

		public override ExpressionType ExpressionType {
			get { return ExpressionType.Call; }
		}

		public bool IsAggregate(IQueryContext context) {
			return false;
		}
	}
}