using System;

namespace LPContribMvc.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
	public class DerivedTypeBinderAwareAttribute : Attribute
	{
		public Type DerivedType
		{
			get;
			private set;
		}

		public DerivedTypeBinderAwareAttribute(Type derivedType)
		{
			DerivedType = derivedType;
		}
	}
}
