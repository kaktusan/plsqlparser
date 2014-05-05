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
using System.Diagnostics;
using System.Text;

using Deveel.Data.DbSystem;
using Deveel.Data.Expressions;

namespace Deveel.Data.Sql.Statements {
	[Serializable]
	[DebuggerDisplay("{ToString(), nq}")]
	public abstract class Statement : IPreparable {
		private bool prepared;

		protected Statement() {
		}

		public SqlQuery Query { get;  internal set; }

		protected virtual void DumpTo(StringBuilder builder) {
		}

		public virtual Statement PrepareExpressions(IExpressionPreparer preparer) {
			return this;
		}

		object IPreparable.Prepare(IExpressionPreparer preparer) {
			return PrepareExpressions(preparer);
		}

		public override string ToString() {
			var builder = new StringBuilder();
			DumpTo(builder);
			return builder.ToString();
		}

		public Statement PrepareStatement(IQueryContext context) {
			var statement = this;

			if (!prepared) {
				statement = Prepare(context);
				prepared = true;

			}
			return statement;
		}

		protected virtual Statement Prepare(IQueryContext context) {
			return this;
		}

		public abstract ITable Evaluate(IQueryContext context);
	}
}