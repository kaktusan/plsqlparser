﻿// 
//  Copyright 2010-2014 Deveel
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
using System.Text;

using Deveel.Data.DbSystem;

namespace Deveel.Data.Index {
	/// <summary>
	/// Represents a base class for a mechanism to select ranges from a 
	/// given set.
	/// </summary>
	/// <remarks>
	/// Such schemes could include BinaryTree, Hashtable or just a blind 
	/// search.
	/// <para>
	/// A given element in the set is specified through a 'row' integer whose
	/// contents can be obtained through the 'table.GetCellContents(column, row)'.
	/// Every scheme is given a table and column number that the set refers to.
	/// While a given set element is refered to as a 'row', the integer is really
	/// only a pointer into the set list which can be de-referenced with a call to
	/// <see cref="ITable.GetValue"/>.  Better performance schemes 
	/// will keep such calls to a minimum.
	/// </para>
	/// <para>
	/// A scheme may choose to retain knowledge about a given element when it is
	/// added or removed from the set, such as a BinaryTree that catalogs all
	/// elements with respect to each other.
	/// </para>
	/// </remarks>
	public abstract class SelectableScheme {
		private static readonly BlockIndex<long> EmptyList;
		private static readonly BlockIndex<long> OneList;

		static SelectableScheme() {
			EmptyList = new BlockIndex<long>();
			EmptyList.IsReadOnly = true;
			OneList = new BlockIndex<long>();
			OneList.Add(0);
			OneList.IsReadOnly = true;
		}

		protected SelectableScheme(ITable table, int column) {
			Table = table;
			Column = column;
		}

		/// <summary>
		/// Returns the Table.
		/// </summary>
		protected ITable Table { get; private set; }

		/// <summary>
		/// Returns the column this scheme is indexing in the table.
		/// </summary>
		protected int Column { get; private set; }

		/// <summary>
		/// Returns true if this scheme is readOnly.
		/// </summary>
		public bool IsReadOnly { get; set; }

		/// <summary>
		/// Obtains the given cell in the row from the table.
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		protected DataObject GetCellContents(long row) {
			return Table.GetValue(Column, row);
		}

		/// <inheritdoc/>
		public override string ToString() {
			// Name of the table
			String tableName = Table.TableInfo.Name.ToString();
			StringBuilder buf = new StringBuilder();
			buf.Append("[ SelectableScheme ");
			buf.Append(base.ToString());
			buf.Append(" for table: ");
			buf.Append(tableName);
			buf.Append("]");

			return buf.ToString();
		}

		/// <summary>
		/// Returns an exact copy of this scheme including any optimization
		/// information.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="immutable"></param>
		/// <remarks>
		/// The copied scheme is identical to the original but does not share 
		/// any parts. Modifying any part of the copied scheme will have no
		/// effect on the original and vice versa.
		/// <para>
		/// The newly copied scheme can be given a new table source. If
		/// 'readOnly' is true, then the resultant scheme is an readOnly 
		/// version of the parent. An readOnly version may share information 
		/// with the copied version so can not be changed.
		/// </para>
		/// <para>
		/// <b>Note</b> Even if the scheme maintains no state you should still 
		/// be careful to ensure a fresh <see cref="SelectableScheme"/> object 
		/// is returned here.
		/// </para>
		/// </remarks>
		/// <returns></returns>
		public abstract SelectableScheme Copy(ITable table, bool immutable);

		/// <summary>
		/// Dispose and invalidate this scheme.
		/// </summary>
		public abstract void Dispose();


		/// <summary>
		/// Inserts the given element into the set.
		/// </summary>
		/// <param name="row"></param>
		/// <remarks>
		/// This is called just after a row has been initially added to a table.
		/// </remarks>
		public abstract void Insert(int row);

		/// <summary>
		/// Removes the given element from the set.
		/// </summary>
		/// <param name="row"></param>
		/// <remarks>
		/// This is called just before the row is removed from the table.
		/// </remarks>
		public abstract void Remove(int row);

		/// <summary>
		/// Sorts the given row set in the order of the scheme.
		/// </summary>
		/// <param name="rowSet"></param>
		/// <remarks>
		/// The values in <paramref name="rowSet"/> must be references to rows in 
		/// the domain of the table this scheme represents.
		/// <para>
		/// The returned set must be stable, meaning if values are equal they 
		/// keep the same ordering.
		/// </para>
		/// <para>
		/// Note that the default implementation of this method can often be 
		/// optimized. For example, <see cref="InsertSearch"/> uses a secondary 
		/// RID list to sort items if the given list is over a certain size.
		/// </para>
		/// </remarks>
		/// <returns>
		/// Returns a <see cref="BlockIndex"/> that represents the given 
		/// <paramref name="rowSet"/> sorted in the order of this scheme.
		/// </returns>
		public IIndex<long> GetOrderedIndex(IList<long> rowSet) {
			// The length of the set to order
			int rowSetLength = rowSet.Count;

			// Trivial cases where sorting is not required:
			// NOTE: We use readOnly objects to save some memory.
			if (rowSetLength == 0)
				return EmptyList;
			if (rowSetLength == 1)
				return OneList;

			// This will be 'row set' sorted by its entry lookup.  This must only
			// contain indices to rowSet entries.
			BlockIndex<long> newSet = new BlockIndex<long>();

			if (rowSetLength <= 250000) {
				// If the subset is less than or equal to 250,000 elements, we generate
				// an array in memory that contains all values in the set and we sort
				// it.  This requires use of memory from the heap but is faster than
				// the no heap use method.
				DataObject[] subsetList = new DataObject[rowSetLength];
				for (int i = 0; i < rowSetLength; ++i) {
					subsetList[i] = GetCellContents(rowSet[i]);
				}

				// The comparator we use to sort
				IIndexComparer<long> comparator = new SubsetIndexComparer(subsetList);

				// Fill new_set with the set { 0, 1, 2, .... , row_set_length }
				for (int i = 0; i < rowSetLength; ++i) {
					DataObject cell = subsetList[i];
					newSet.InsertSort(cell, i, comparator);
				}

			} else {
				// This is the no additional heap use method to sorting the sub-set.

				// The comparator we use to sort
				IIndexComparer<long> comparator = new SchemeIndexComparer(this, rowSet);

				// Fill new_set with the set { 0, 1, 2, .... , row_set_length }
				for (int i = 0; i < rowSetLength; ++i) {
					DataObject cell = GetCellContents(rowSet[i]);
					newSet.InsertSort(cell, i, comparator);
				}
			}

			return newSet;

		}

		/// <summary>
		/// Asks the <see cref="SelectableScheme">scheme</see> for a 
		/// <see cref="SelectableScheme"/> object that describes a 
		/// sub-set of the set handled by this scheme.
		/// </summary>
		/// <param name="subsetTable"></param>
		/// <param name="subsetColumn"></param>
		/// <remarks>
		/// Since a <see cref="Table">table</see> stores a subset of a given 
		/// <see cref="DataTable"/>, we pass this as the argument.  It returns 
		/// a new <see cref="SelectableScheme"/> that orders the rows in the 
		/// given columns order.
		/// The  <see cref="Column"/> variable specifies the column index of 
		/// this column in the given table.
		/// </remarks>
		/// <returns></returns>
		public SelectableScheme GetSubsetScheme(Table subsetTable, int subsetColumn) {
			// Resolve table rows in this table scheme domain.
			List<long> rowSet = new List<long>((int)subsetTable.RowCount);
			IEnumerator<long> e = subsetTable.GetRowEnumerator();
			while (e.MoveNext()) {
				rowSet.Add(e.Current);
			}

			subsetTable.SetToRowTableDomain(subsetColumn, rowSet, Table);

			// Generates an IIndex which contains indices into 'rowSet' in
			// sorted order.
			IIndex<long> newSet = GetOrderedIndex(rowSet);

			// Our 'new_set' should be the same size as 'rowSet'
			if (newSet.Count != rowSet.Count) {
				throw new Exception("Internal sort error in finding sub-set.");
			}

			// Set up a new SelectableScheme with the sorted index set.
			// Move the sorted index set into the new scheme.
			InsertSearch scheme = new InsertSearch(subsetTable, subsetColumn, newSet);
			// Don't let subset schemes create uid caches.
			scheme.RecordUid = false;
			return scheme;

		}

		/**
		 * These are the select operations that are the main purpose of the scheme.
		 * They retrieve the given information from the set.  Different schemes will
		 * have varying performance on different types of data sets.
		 * The select operations must *always* return a resultant row set that
		 * is sorted from lowest to highest.
		 */

		///<summary>
		///</summary>
		///<returns></returns>
		public virtual IList<long> SelectAll() {
			return SelectRange(new SelectableRange(
					 RangePosition.FirstValue, SelectableRange.FirstInSet,
					 RangePosition.LastValue, SelectableRange.LastInSet));
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public virtual IList<long> SelectFirst() {
			// NOTE: This will find NULL at start which is probably wrong.  The
			//   first value should be the first non null value.
			return SelectRange(new SelectableRange(
					 RangePosition.FirstValue, SelectableRange.FirstInSet,
					 RangePosition.LastValue, SelectableRange.FirstInSet));
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public IList<long> SelectNotFirst() {
			// NOTE: This will find NULL at start which is probably wrong.  The
			//   first value should be the first non null value.
			return SelectRange(new SelectableRange(
					 RangePosition.AfterLastValue, SelectableRange.FirstInSet,
					 RangePosition.LastValue, SelectableRange.LastInSet));
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public IList<long> SelectLast() {
			return SelectRange(new SelectableRange(
					 RangePosition.FirstValue, SelectableRange.LastInSet,
					 RangePosition.LastValue, SelectableRange.LastInSet));
		}

		///<summary>
		///</summary>
		///<returns></returns>
		public IList<long> SelectNotLast() {
			return SelectRange(new SelectableRange(
					 RangePosition.FirstValue, SelectableRange.FirstInSet,
					 RangePosition.BeforeFirstValue, SelectableRange.LastInSet));
		}

		///<summary>
		/// Selects all values in the column that are not null.
		///</summary>
		///<returns></returns>
		public IList<long> SelectAllNonNull() {
			return SelectRange(new SelectableRange(
						 RangePosition.AfterLastValue, DataObject.Null,
						 RangePosition.LastValue, SelectableRange.LastInSet));
		}

		///<summary>
		///</summary>
		///<param name="ob"></param>
		///<returns></returns>
		public IList<long> SelectEqual(DataObject ob) {
			if (ob.IsNull)
				return new List<long>(0);

			return SelectRange(new SelectableRange(
								 RangePosition.FirstValue, ob,
								 RangePosition.LastValue, ob));
		}

		///<summary>
		///</summary>
		///<param name="ob"></param>
		///<returns></returns>
		public IList<long> SelectNotEqual(DataObject ob) {
			if (ob.IsNull) {
				return new List<long>(0);
			}
			return SelectRange(new SelectableRange[]
			                   	{
			                   		new SelectableRange(
			                   			RangePosition.AfterLastValue, DataObject.Null,
			                   			RangePosition.BeforeFirstValue, ob)
			                   		, new SelectableRange(
			                   		  	RangePosition.AfterLastValue, ob,
			                   		  	RangePosition.LastValue, SelectableRange.LastInSet)
			                   	});
		}

		///<summary>
		///</summary>
		///<param name="ob"></param>
		///<returns></returns>
		public IList<long> SelectGreater(DataObject ob) {
			if (ob.IsNull)
				return new List<long>(0);

			return SelectRange(new SelectableRange(
					   RangePosition.AfterLastValue, ob,
					   RangePosition.LastValue, SelectableRange.LastInSet));
		}

		///<summary>
		///</summary>
		///<param name="ob"></param>
		///<returns></returns>
		public IList<long> SelectLess(DataObject ob) {
			if (ob.IsNull)
				return new List<long>(0);

			return SelectRange(new SelectableRange(
					   RangePosition.AfterLastValue, DataObject.Null,
					   RangePosition.BeforeFirstValue, ob));
		}

		///<summary>
		///</summary>
		///<param name="ob"></param>
		///<returns></returns>
		public IList<long> SelectGreaterOrEqual(DataObject ob) {
			if (ob.IsNull)
				return new List<long>(0);

			return SelectRange(new SelectableRange(
					   RangePosition.FirstValue, ob,
					   RangePosition.LastValue, SelectableRange.LastInSet));
		}

		///<summary>
		///</summary>
		///<param name="ob"></param>
		///<returns></returns>
		public IList<long> SelectLessOrEqual(DataObject ob) {
			if (ob.IsNull)
				return new List<long>(0);

			return SelectRange(new SelectableRange(
					   RangePosition.AfterLastValue, DataObject.Null,
					   RangePosition.LastValue, ob));
		}

		// Inclusive of rows that are >= ob1 and < ob2
		// NOTE: This is not compatible with SQL BETWEEN predicate which is all
		//   rows that are >= ob1 and <= ob2
		///<summary>
		///</summary>
		///<param name="ob1"></param>
		///<param name="ob2"></param>
		///<returns></returns>
		public IList<long> SelectBetween(DataObject ob1, DataObject ob2) {
			if (ob1.IsNull || ob2.IsNull)
				return new List<long>(0);

			return SelectRange(new SelectableRange(
					   RangePosition.FirstValue, ob1,
					   RangePosition.BeforeFirstValue, ob2));
		}

		/// <summary>
		/// Selects the given range of values from this index.
		/// </summary>
		/// <param name="range"></param>
		/// <remarks>
		/// The <see cref="SelectableRange"/> must contain a 
		/// <see cref="SelectableRange.Start"/> value that compares &lt;= to 
		/// the <see cref="SelectableRange.End"/> value.
		/// <para>
		/// This must guarentee that the returned set is sorted from lowest to
		/// highest value.
		/// </para>
		/// </remarks>
		/// <returns></returns>
		public abstract IList<long> SelectRange(SelectableRange range);

		/// <summary>
		/// Selects a set of ranges from this index.
		/// </summary>
		/// <param name="ranges"></param>
		/// <remarks>
		/// The ranges must not overlap and each range must contain a 'start' 
		/// value that compares &lt;= to the 'end' value. Every range in the 
		/// array must represent a range that's lower than the preceeding range 
		/// (if it exists).
		/// <para>
		/// If the above rules are enforced (as they must be) then this method 
		/// will return a set that is sorted from lowest to highest value.
		/// </para>
		/// <para>
		/// This must guarentee that the returned set is sorted from lowest to
		/// highest value.
		/// </para>
		/// </remarks>
		/// <returns></returns>
		public abstract IList<long> SelectRange(SelectableRange[] ranges);

		private class SubsetIndexComparer : IIndexComparer<long> {
			private readonly DataObject[] subsetList;

			public SubsetIndexComparer(DataObject[] subsetList) {
				this.subsetList = subsetList;
			}

			public int CompareValue(long index, DataObject val) {
				DataObject cell = subsetList[index];
				return cell.CompareTo(val);
			}

			public int Compare(long index1, long index2) {
				throw new NotSupportedException("Shouldn't be called!");
			}
		}

		private class SchemeIndexComparer : IIndexComparer<long> {
			private readonly SelectableScheme scheme;
			private readonly IList<long> rowSet;

			public SchemeIndexComparer(SelectableScheme scheme, IList<long> rowSet) {
				this.scheme = scheme;
				this.rowSet = rowSet;
			}

			public int CompareValue(long index, DataObject val) {
				DataObject cell = scheme.GetCellContents(rowSet[(int)index]);
				return cell.CompareTo(val);
			}

			public int Compare(long index1, long index2) {
				throw new NotSupportedException("Shouldn't be called!");
			}
		}
	}
}