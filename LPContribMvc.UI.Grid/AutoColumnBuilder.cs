using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace LPContribMvc.UI.Grid
{
	public class AutoColumnBuilder<T> : ColumnBuilder<T> where T : class
	{
		private readonly ModelMetadataProvider _metadataProvider;

		public AutoColumnBuilder(ModelMetadataProvider metadata)
		{
			_metadataProvider = metadata;
			BuildColumns();
		}

		private void BuildColumns()
		{
			ModelMetadata metadataForType = _metadataProvider.GetMetadataForType((Func<object>)(() => null), typeof(T));
			foreach (ModelMetadata property in metadataForType.Properties)
			{
				if (property.ShowForDisplay)
				{
					IGridColumn<T> gridColumn = For(PropertyToExpression(property));
					if (!string.IsNullOrEmpty(property.DisplayName))
					{
						gridColumn.Named(property.DisplayName);
					}
					if (!string.IsNullOrEmpty(property.DisplayFormatString))
					{
						gridColumn.Format(property.DisplayFormatString);
					}
				}
			}
		}

		private Expression<Func<T, object>> PropertyToExpression(ModelMetadata property)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
			Expression expression = Expression.Property(parameterExpression, property.PropertyName);
			if (property.ModelType.IsValueType)
			{
				expression = Expression.Convert(expression, typeof(object));
			}
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Func<T, object>), expression, parameterExpression);
			return (Expression<Func<T, object>>)lambdaExpression;
		}
	}
}
