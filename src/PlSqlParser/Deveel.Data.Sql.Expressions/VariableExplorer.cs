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
using System.Collections.ObjectModel;
using System.Linq;

namespace Deveel.Data.Sql.Expressions {
	static class VariableExplorer {
		public static IEnumerable<ObjectName> AllVariables(Expression expression) {
			var visitor = new VariableVisitor();
			visitor.VisitExpression(expression);
			return visitor.AllVariables.AsEnumerable();
		}

		#region VariableVisitor

		class VariableVisitor : ExpressionVisitor {
			public VariableVisitor() {
				AllVariables = new Collection<ObjectName>();
			}

			public ICollection<ObjectName> AllVariables { get; private set; }

			public void VisitExpression(Expression expression) {
				Visit(expression);
			}

			protected override Expression VisitVariable(VariableExpression p) {
				if (!AllVariables.Contains(p.VariableName))
					AllVariables.Add(p.VariableName);

				return base.VisitVariable(p);
			}
		}

		#endregion
	}
}