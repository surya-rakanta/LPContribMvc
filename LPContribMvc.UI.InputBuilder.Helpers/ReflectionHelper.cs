using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LPContribMvc.UI.InputBuilder.Helpers
{
	public static class ReflectionHelper
	{
		public static string ToSeparatedWords(this string value)
		{
			return Regex.Replace(value, "([A-Z][a-z])", " $1").Trim();
		}

		public static PropertyInfo FindProperty(LambdaExpression lambdaExpression)
		{
			Expression expression = lambdaExpression;
			bool flag = false;
			while (!flag)
			{
				switch (expression.NodeType)
				{
				case ExpressionType.Convert:
					expression = ((UnaryExpression)expression).Operand;
					break;
				case ExpressionType.Lambda:
					expression = lambdaExpression.Body;
					break;
				case ExpressionType.MemberAccess:
					return ((MemberExpression)expression).Member as PropertyInfo;
				default:
					flag = true;
					break;
				}
			}
			return null;
		}

		public static string BuildNameFrom(Expression expression)
		{
			Expression expression2 = expression;
			List<string> list = new List<string>();
			bool flag = false;
			bool flag2 = false;
			while (!flag)
			{
				switch (expression2.NodeType)
				{
				case ExpressionType.Convert:
					flag2 = false;
					expression2 = ((UnaryExpression)expression2).Operand;
					break;
				case ExpressionType.ArrayIndex:
				{
					BinaryExpression binaryExpression = (BinaryExpression)expression2;
					Expression right = binaryExpression.Right;
					Delegate @delegate = Expression.Lambda(right).Compile();
					int num = (int)@delegate.DynamicInvoke();
					if (flag2)
					{
						list.Add(".");
					}
					list.Add($"[{num}]");
					flag2 = false;
					expression2 = binaryExpression.Left;
					break;
				}
				case ExpressionType.Lambda:
				{
					LambdaExpression lambdaExpression = (LambdaExpression)expression2;
					flag2 = false;
					expression2 = lambdaExpression.Body;
					break;
				}
				case ExpressionType.MemberAccess:
				{
					MemberExpression memberExpression = (MemberExpression)expression2;
					if (flag2)
					{
						list.Add(".");
					}
					list.Add(memberExpression.Member.Name);
					if (memberExpression.Expression == null)
					{
						flag = true;
						break;
					}
					flag2 = true;
					expression2 = memberExpression.Expression;
					break;
				}
				default:
					flag = true;
					break;
				}
			}
			list.Reverse();
			return string.Join(string.Empty, list.ToArray());
		}

		public static PropertyInfo FindPropertyFromExpression(LambdaExpression lambdaExpression)
		{
			Expression expression = lambdaExpression;
			List<string> list = new List<string>();
			bool flag = false;
			bool flag2 = false;
			while (!flag)
			{
				switch (expression.NodeType)
				{
				case ExpressionType.Convert:
					expression = ((UnaryExpression)expression).Operand;
					break;
				case ExpressionType.Lambda:
					expression = lambdaExpression.Body;
					break;
				case ExpressionType.MemberAccess:
					return ((MemberExpression)expression).Member as PropertyInfo;
				case ExpressionType.ArrayIndex:
				{
					BinaryExpression binaryExpression = (BinaryExpression)expression;
					Expression right = binaryExpression.Right;
					Delegate @delegate = Expression.Lambda(right).Compile();
					int num = (int)@delegate.DynamicInvoke();
					if (flag2)
					{
						list.Add(".");
					}
					list.Add($"[{num}]");
					flag2 = false;
					expression = binaryExpression.Left;
					break;
				}
				default:
					flag = true;
					break;
				}
			}
			return null;
		}

		public static bool IsIndexed(LambdaExpression lambdaExpression)
		{
			Expression expression = lambdaExpression;
			bool flag = false;
			while (!flag)
			{
				switch (expression.NodeType)
				{
				case ExpressionType.Convert:
					expression = ((UnaryExpression)expression).Operand;
					break;
				case ExpressionType.Lambda:
					expression = ((LambdaExpression)expression).Body;
					break;
				case ExpressionType.MemberAccess:
					return false;
				case ExpressionType.ArrayIndex:
					return true;
				default:
					flag = true;
					break;
				}
			}
			return false;
		}
	}
}
