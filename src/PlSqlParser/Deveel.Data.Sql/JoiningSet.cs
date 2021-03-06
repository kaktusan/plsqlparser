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

using Deveel.Data.Sql.Expressions;

namespace Deveel.Data.Sql {
	public sealed class JoiningSet {
		private readonly List<object> joinSet;

		public JoiningSet() {
			joinSet = new List<object>();
		}

		public void AddTable(string tableName) {
			joinSet.Add(tableName);
		}

		public void AddPreviousJoin(JoinType type, Expression onExpression) {
			joinSet.Insert(joinSet.Count - 1, new JoinPart(type, onExpression));
		}

		public void AddJoin(JoinType type, Expression onExpression) {
			joinSet.Add(new JoinPart(type, onExpression));
		}

		public void AddJoin(JoinType type) {
			joinSet.Add(new JoinPart(type));
		}

		public int TableCount {
			get { return (joinSet.Count + 1) / 2; }
		}

		public string FirstTable {
			get { return this[0]; }
		}

		public string this[int n] {
			get { return (string)joinSet[n * 2]; }
		}

		private void SetTable(int n, string table) {
			joinSet[n * 2] = table;
		}

		public JoinType GetJoinType(int n) {
			return ((JoinPart)joinSet[(n * 2) + 1]).Type;
		}

		public Expression GetOnExpression(int n) {
			return ((JoinPart)joinSet[(n * 2) + 1]).OnExpression;
		}

		private sealed class JoinPart {
			public readonly JoinType Type;
			public readonly Expression OnExpression;

			public JoinPart(JoinType type, Expression onExpression) {
				this.Type = type;
				this.OnExpression = onExpression;
			}

			public JoinPart(JoinType type)
				: this(type, null) {
			}
		}

		public JoiningSet Prepare(IExpressionPreparer preparer) {
			var joiningSet = new JoiningSet();
			foreach (object obj in joinSet) {
				if (obj is Expression) {
					var exp = obj as Expression;
					joiningSet.joinSet.Add(exp.Prepare(preparer));
				} else {
					joiningSet.joinSet.Add(obj);
				}
			}

			return joiningSet;
		}
	}
}