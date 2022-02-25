using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace LPContribMvc.UI.Grid
{
	public class ColumnBuilder<T> : IList<GridColumn<T>>, ICollection<GridColumn<T>>, IEnumerable<GridColumn<T>>, IEnumerable where T : class
	{
		private readonly ModelMetadataProvider _metadataProvider;

		private readonly List<GridColumn<T>> _columns = new List<GridColumn<T>>();

		protected IList<GridColumn<T>> Columns => _columns;

		int ICollection<GridColumn<T>>.Count => _columns.Count;

		bool ICollection<GridColumn<T>>.IsReadOnly => false;

		GridColumn<T> IList<GridColumn<T>>.this[int index]
		{
			get
			{
				return _columns[index];
			}
			set
			{
				_columns[index] = value;
			}
		}

		public ColumnBuilder()
			: this(ModelMetadataProviders.Current)
		{
		}

		private int _treeDepth = 0;

		private Stack<GridColumn<T>> _parentColumn = new Stack<GridColumn<T>>();

		private ColumnBuilder(ModelMetadataProvider metadataProvider)
		{
			_metadataProvider = metadataProvider;
		}
		public int TreeDepth => _treeDepth;

		public IGridColumn<T> Custom(Func<T, object> customRenderer)
		{
			GridColumn<T> gridColumn = new GridColumn<T>(customRenderer, "", typeof(object));
			gridColumn.Encode(shouldEncode: false);
			Add(gridColumn);
			return gridColumn;
		}

		public IGridColumn<T> ChildColumns(Action<ColumnBuilder<T>> columnBuilder)
		{
			columnBuilder(this);
			return _parentColumn.Pop();
		}

		public IGridColumn<T> For(Expression<Func<T, object>> propertySpecifier)
		{
			MemberExpression memberExpression = GetMemberExpression(propertySpecifier);
			Type typeFromMemberExpression = GetTypeFromMemberExpression(memberExpression);
			Type type = memberExpression?.Expression.Type;
			string text = memberExpression?.Member.Name;
			GridColumn<T> gridColumn = new GridColumn<T>(propertySpecifier.Compile(), text, typeFromMemberExpression);
			if (type != null)
			{
				ModelMetadata metadataForProperty = _metadataProvider.GetMetadataForProperty((Func<object>)null, type, text);
				if (!string.IsNullOrEmpty(metadataForProperty.DisplayName))
				{
					gridColumn.Named(metadataForProperty.DisplayName);
				}
				if (!string.IsNullOrEmpty(metadataForProperty.DisplayFormatString))
				{
					gridColumn.Format(metadataForProperty.DisplayFormatString);
				}
			}

			if (_parentColumn.Count > 0)
			{
				_parentColumn.Peek().AddChild(gridColumn);
			} 
			else
			{
				Add(gridColumn);
			}

			return gridColumn;
		}

		public IEnumerator<GridColumn<T>> GetEnumerator()
		{
			return _columns.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public static MemberExpression GetMemberExpression(LambdaExpression expression)
		{
			return RemoveUnary(expression.Body) as MemberExpression;
		}

		private static Type GetTypeFromMemberExpression(MemberExpression memberExpression)
		{
			if (memberExpression == null)
			{
				return null;
			}
			Type typeFromMemberInfo = GetTypeFromMemberInfo(memberExpression.Member, (PropertyInfo p) => p.PropertyType);
			if (typeFromMemberInfo == null)
			{
				typeFromMemberInfo = GetTypeFromMemberInfo(memberExpression.Member, (MethodInfo m) => m.ReturnType);
			}
			if (typeFromMemberInfo == null)
			{
				typeFromMemberInfo = GetTypeFromMemberInfo(memberExpression.Member, (FieldInfo f) => f.FieldType);
			}
			return typeFromMemberInfo;
		}

		private static Type GetTypeFromMemberInfo<TMember>(MemberInfo member, Func<TMember, Type> func) where TMember : MemberInfo
		{
			if (member is TMember)
			{
				return func((TMember)member);
			}
			return null;
		}

		private static Expression RemoveUnary(Expression body)
		{
			UnaryExpression unaryExpression = body as UnaryExpression;
			if (unaryExpression != null)
			{
				return unaryExpression.Operand;
			}
			return body;
		}

		//private static int nCnt = 0;

		public virtual ColumnBuilder<T> Add()
		{
			GridColumn<T> gridColumn = new GridColumn<T>(null, "&nbsp;", null);

			//check whether we are not in root position
			if (_parentColumn.Count>0)
			{
				_parentColumn.Peek().AddChild(gridColumn);
			} 
			else
			{
				_columns.Add(gridColumn);
			}
			_parentColumn.Push(gridColumn);

			_treeDepth = Math.Max(_treeDepth, _parentColumn.Count);

			return this;
		}

		protected virtual void Add(GridColumn<T> column)
		{
			_columns.Add(column);
		}

		void ICollection<GridColumn<T>>.Add(GridColumn<T> column)
		{
			Add(column);
		}

		void ICollection<GridColumn<T>>.Clear()
		{
			_columns.Clear();
		}

		bool ICollection<GridColumn<T>>.Contains(GridColumn<T> column)
		{
			return _columns.Contains(column);
		}

		void ICollection<GridColumn<T>>.CopyTo(GridColumn<T>[] array, int arrayIndex)
		{
			_columns.CopyTo(array, arrayIndex);
		}

		bool ICollection<GridColumn<T>>.Remove(GridColumn<T> column)
		{
			return _columns.Remove(column);
		}

		int IList<GridColumn<T>>.IndexOf(GridColumn<T> item)
		{
			return _columns.IndexOf(item);
		}

		void IList<GridColumn<T>>.Insert(int index, GridColumn<T> item)
		{
			_columns.Insert(index, item);
		}

		void IList<GridColumn<T>>.RemoveAt(int index)
		{
			_columns.RemoveAt(index);
		}
	}
}
