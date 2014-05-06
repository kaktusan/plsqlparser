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

namespace Deveel.Data.DbSystem {
	public class DatabaseQueryContext : QueryContext {
		private readonly IDatabaseConnection connection;

		public DatabaseQueryContext(IDatabaseConnection connection) {
			this.connection = connection;
		}

		public override IDatabaseConnection Connection {
			get { return connection; }
		}

		internal IQueryPlanNode CreateViewQueryPlanNode(ObjectName viewName) {
			throw new NotImplementedException();
		}
	}
}