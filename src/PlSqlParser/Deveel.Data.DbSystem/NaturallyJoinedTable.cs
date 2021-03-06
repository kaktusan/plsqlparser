// 
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

using Deveel.Math;

namespace Deveel.Data.DbSystem {
	/// <summary>
	/// A table that is the cartesian product of two tables.
	/// </summary>
	/// <remarks>
	/// Provides better memory-use and efficiency than a materialized table 
	/// backed by a <see cref="VirtualTable"/>.
	/// </remarks>
	public sealed class NaturallyJoinedTable : JoinedTable {
		// The row counts of the left and right tables.
		private readonly long leftRowCount, rightRowCount;

		// The lookup row set for the left and right tables.  Basically, these point
		// to each row in either the left or right tables.
		private readonly IList<long> leftSet, rightSet;
		private readonly bool leftIsSimpleEnum, rightIsSimpleEnum;

		///<summary>
		///</summary>
		///<param name="left"></param>
		///<param name="right"></param>
		public NaturallyJoinedTable(Table left, Table right) {
			base.Init(new Table[] { left, right });

			leftRowCount = left.RowCount;
			rightRowCount = right.RowCount;

			// Build lookup tables for the rows in the parent tables if necessary
			// (usually it's not necessary).

			// If the left or right tables are simple enumerations, we can optimize
			// our access procedure,
			leftIsSimpleEnum = (left.GetRowEnumerator() is SimpleRowEnumerator);
			rightIsSimpleEnum = (right.GetRowEnumerator() is SimpleRowEnumerator);

			leftSet = !leftIsSimpleEnum ? CreateLookupRowList(left) : null;
			rightSet = !rightIsSimpleEnum ? CreateLookupRowList(right) : null;

		}

		/// <summary>
		/// Creates a lookup list for rows in the given table.
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		private static IList<long> CreateLookupRowList(ITable t) {
			List<long> ivec = new List<long>();
			IEnumerator<long> en = t.GetRowEnumerator();
			while (en.MoveNext()) {
				long rowIndex = en.Current;
				ivec.Add(rowIndex);
			}
			return ivec;
		}

		/// <summary>
		/// Given a row index between 0 and left table row count, this will return a
		/// row index into the left table's row domain.
		/// </summary>
		/// <param name="rowIndex"></param>
		/// <returns></returns>
		private long GetLeftRowIndex(long rowIndex) {
			if (leftIsSimpleEnum)
				return rowIndex;

			return leftSet[(int)rowIndex];
		}

		/// <summary>
		/// Given a row index between 0 and right table row count, this will return a
		/// row index into the right table's row domain.
		/// </summary>
		/// <param name="rowIndex"></param>
		/// <returns></returns>
		private long GetRightRowIndex(long rowIndex) {
			if (rightIsSimpleEnum)
				return rowIndex;

			return rightSet[(int)rowIndex];
		}

		public override long RowCount {
			get {
				// Natural join row count is (left table row count * right table row count)
				return leftRowCount*rightRowCount;
			}
		}

		protected override long ResolveRowForTableAt(long rowNumber, int tableNum) {
			if (tableNum == 0)
				return GetLeftRowIndex(rowNumber/rightRowCount);
			return GetRightRowIndex(rowNumber % rightRowCount);
		}

		protected override void ResolveAllRowsForTableAt(IList<long> rowSet, int tableNum) {
			bool pickRightTable = (tableNum == 1);
			for (int n = rowSet.Count - 1; n >= 0; --n) {
				long aa = rowSet[n];
				// Reverse map row index to parent domain
				long parent_row;
				if (pickRightTable) {
					parent_row = GetRightRowIndex(aa % rightRowCount);
				} else {
					parent_row = GetLeftRowIndex(aa / rightRowCount);
				}
				rowSet[n] = parent_row;
			}
		}
	}
}