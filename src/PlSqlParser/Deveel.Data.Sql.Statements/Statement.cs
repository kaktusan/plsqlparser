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
using Deveel.Data.Sql.Expressions;

namespace Deveel.Data.Sql.Statements {
	[Serializable]
	[DebuggerDisplay("{ToString(), nq}")]
	public abstract class Statement : IPreparable, ISqlElement {
		private bool prepared;

		protected Statement() {
		}

		public SqlQuery Query { get;  internal set; }

		public virtual Statement PrepareExpressions(IExpressionPreparer preparer) {
			return this;
		}

		object IPreparable.Prepare(IExpressionPreparer preparer) {
			return PrepareExpressions(preparer);
		}

		protected virtual void WriteTo(ISqlWriter writer) {
			
		}

		public override string ToString() {
			var writer = new StringSqlWriter();
			WriteTo(writer);
			return writer.ToString();
		}

		void ISqlElement.ToString(ISqlWriter writer) {
			WriteTo(writer);
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